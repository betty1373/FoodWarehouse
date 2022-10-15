using FW.BusinessLogic.Services.Abstractions;
using FW.EntityFramework;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.ChangesProducts;
using AutoMapper;


namespace FW.BusinessLogic.Services
{
    public class CookingDishService : ICookingDishService
    {
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public CookingDishService(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // ToDo: сделать рефакторинг кода
        public async Task<ResponseStatusResult> Cook(Guid dishId, Guid userId, int numPortions)
        {
            // заготовка результата приготовления
            var result = new ResponseStatusResult
            {
                Id = dishId,
                Status = StatusResult.NotFound
            };

            // проверка существования блюда
            var dish = await FirstOrDefaultAsync(dishId,userId);
            if (dish == null)
            {
                result.Title = "Dish not found";
                return result;
            }

            var warehouse = await _dbContext.Warehouses.Where(p => EF.Property<Guid?>(p, "UserId").Equals(userId)).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                result.Title = "Warehouse for this user not found";
                return result;
            }

            // получение Id ингредиентов и их количества для блюда
            var quantityIngredientsDish = await _dbContext.Recipes
                .Where(r => r.DishesId == dishId)
                .ToDictionaryAsync(r => r.IngredientId, r => r.Quantity);
            if (quantityIngredientsDish == null)
            {
                result.Title = $"Not found dish {dish.Name} ingredients";
                return result;
            }

            // получение продуктов, которые соответствуют ингредиентам блюда и есть в наличии с действующим сроком годности 
            var dateNow = DateTime.UtcNow;
            var products = await _dbContext.Products
                .Where(p => p.WarehouseId == warehouse.Id &&
                       quantityIngredientsDish.Keys.Contains(p.IngredientId) &&
                       p.Quantity > 0 &&
                       p.ExpirationDate.Date >= dateNow.Date)
                .OrderBy(p => p.ExpirationDate)
                .ToListAsync();
            if (products == null)
            {
                result.Title = $"Not found products in food warehouse";
                return result;
            }

            // групорование продуктов по ингредиентам, т.к. некоторые продукты(ингредиенты на складе) могут быть с разными сроками годности и разным количеством)
            var groupsProducts = products.GroupBy(x => x.IngredientId);

            // проверка все ли ингредиенты по рецепту в необходимом количестве
            var quantityProductsForDishInStock = GetQuantityProductsForDishInStock(groupsProducts, quantityIngredientsDish, numPortions);
            if (quantityProductsForDishInStock.Count != quantityIngredientsDish.Count)
            {
                var namesOfMissingIngredients = await GetNamesOfMissingIngredients(quantityProductsForDishInStock, quantityIngredientsDish);

                result.Status = StatusResult.Error;
                result.Title = $"The following ingredients are not enough for cooking dish {dish.Name}: {namesOfMissingIngredients}.";
                return result;
            }

            // списание продуктов с занеcением изменений количества в таблицу БД `ChangesProducts`
            foreach (var groupProducts in groupsProducts)
            {
                var ingredientId = groupProducts.First().IngredientId;
                var quantityProduct = quantityProductsForDishInStock[ingredientId];

                foreach (var product in groupProducts)
                {
                    if (product.Quantity >= quantityProduct)
                    {
                        product.Quantity -= quantityProduct;
                        await ChangesProductCreate(product, quantityProduct,userId);
                        break;
                    }
                    else
                    {
                        quantityProduct -= product.Quantity;
                        await ChangesProductCreate(product, product.Quantity, userId);
                        product.Quantity = 0;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            result.Status = StatusResult.Ok;
            result.Title = $"Dish {dish.Name} was cooked";
            return result;
        }

        // добавление в журнал изменения продуктов
        private async Task ChangesProductCreate(Products product, int quantity,Guid userId)
        {          
            var dto = new ChangesProductCreateDto
            {
                ProductId = product.Id,
                Quantity = -quantity,
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
            };
            var item = _mapper.Map<ChangesProducts>(dto);
            await _dbContext.ChangesProducts.AddAsync(item);
        }

        // проверка все ли продукты по рецепту в необходимом количестве
        private Dictionary<Guid, int> GetQuantityProductsForDishInStock(IEnumerable<IGrouping<Guid, Products>> groupsProducts,
            Dictionary<Guid, int> quantityIngredientsDish, int numPortions)
        {
            var result = new Dictionary<Guid, int>(groupsProducts.Count());
            foreach (var groupProduct in groupsProducts)
            {
                var ingredientId = groupProduct.First().IngredientId;
                var totalQuantityProduct = groupProduct.Sum(x => x.Quantity);
                var totalQuantityIngredientDish = quantityIngredientsDish[ingredientId] * numPortions;

                if (totalQuantityProduct >= totalQuantityIngredientDish)
                    result.Add(ingredientId, totalQuantityIngredientDish);
            }
            return result;
        }

        // получение названия ингредиентов, которых не хватает
        private async Task<string> GetNamesOfMissingIngredients(Dictionary<Guid, int> necessaryQuantityProducts, Dictionary<Guid, int> quantityIngredientsDish)
        {
            // получаем список Id ингредиентов, которых не хватает
            var difference = quantityIngredientsDish.Keys.ToArray()
                .Except(necessaryQuantityProducts.Keys.ToArray());

            // получаем ингредиенты, которых не хватает
            var ingredients = await _dbContext.Ingredients
                .Where(i => difference.Contains(i.Id))
                .Select(i => i.Name)
                .ToArrayAsync();

            return string.Join(", ", ingredients);
        }

        private async Task<Dishes?> FirstOrDefaultAsync(Guid id,Guid userId)
        {
            return await _dbContext.Dishes
                .Where(p => p.Id.Equals(id) && EF.Property<Guid?>(p, "UserId").Equals(userId))
                .FirstOrDefaultAsync();
        }
    }
}
