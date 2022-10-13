using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Management.EventHandlers.ChangesProducts;
using FW.Management.EventHandlers.Dishes;
using FW.Management.EventHandlers.Ingredients;
using FW.Management.EventHandlers.Recipes;
using FW.Management.EventHandlers.Warehouses;
using FW.RabbitMQOptions;

namespace FW.Management.Configurations
{
    public static class WebApplicationConfiguration
    {
        public static WebApplication Configure(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            var eventBus = app.Services.GetRequiredService<IEventBus>();

            var exchangeNames = builder.Configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            var queueNames = builder.Configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();

            eventBus.Subscribe<ChangesProductGetByIdEventHandler, ChangesProductGetByIdDto>(queueNames.ChangesProducts.Get, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductsGetByParentIdEventHandler, ChangesProductsGetByParentIdDto>(queueNames.ChangesProducts.GetByParentId, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductsGetPageEventHandler, ChangesProductGetPageDto>(queueNames.ChangesProducts.GetPage, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductsGetAllEventHandler, ChangesProductsGetAllDto>(queueNames.ChangesProducts.GetAll, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductsGetCountEventHandler, ChangesProductsGetCountDto>(queueNames.ChangesProducts.Count, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductCreateEventHandler, ChangesProductCreateDto>(queueNames.ChangesProducts.Create, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductUpdateEventHandler, ChangesProductUpdateDto>(queueNames.ChangesProducts.Update, exchangeNames.ChangesProducts);
            eventBus.Subscribe<ChangesProductDeleteEventHandler, ChangesProductDeleteDto>(queueNames.ChangesProducts.Delete, exchangeNames.ChangesProducts);

            eventBus.Subscribe<DishGetByIdEventHandler, DishGetByIdDto>(queueNames.Dishes.Get, exchangeNames.Dishes);
            eventBus.Subscribe<DishesGetPageEventHandler, DishesGetPageDto>(queueNames.Dishes.GetPage, exchangeNames.Dishes);
            eventBus.Subscribe<DishesGetAllEventHandler, DishesGetAllDto>(queueNames.Dishes.GetAll, exchangeNames.Dishes);
            eventBus.Subscribe<DishesGetCountEventHandler, DishesGetCountDto>(queueNames.Dishes.Count, exchangeNames.Dishes);
            eventBus.Subscribe<DishCreateEventHandler, DishCreateDto>(queueNames.Dishes.Create, exchangeNames.Dishes);
            eventBus.Subscribe<DishUpdateEventHandler, DishUpdateDto>(queueNames.Dishes.Update, exchangeNames.Dishes);
            eventBus.Subscribe<DishDeleteEventHandler, DishDeleteDto>(queueNames.Dishes.Delete, exchangeNames.Dishes);
            eventBus.Subscribe<DishesGetByParentIdEventHandler, DishesGetByParentIdDto>(queueNames.Dishes.GetByParentId, exchangeNames.Dishes);
            eventBus.Subscribe<DishCookEventHandler, DishCookDto>(queueNames.Dishes.Cook, exchangeNames.Dishes);

            eventBus.Subscribe<IngredientGetByIdEventHandler, IngredientGetByIdDto>(queueNames.Ingredients.Get, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientsGetPageEventHandler, IngredientsGetPageDto>(queueNames.Ingredients.GetPage, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientsGetAllEventHandler, IngredientsGetAllDto>(queueNames.Ingredients.GetAll, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientsGetCountEventHandler, IngredientsGetCountDto>(queueNames.Ingredients.Count, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientCreateEventHandler, IngredientCreateDto>(queueNames.Ingredients.Create, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientUpdateEventHandler, IngredientUpdateDto>(queueNames.Ingredients.Update, exchangeNames.Ingredients);
            eventBus.Subscribe<IngredientDeleteEventHandler, IngredientDeleteDto>(queueNames.Ingredients.Delete, exchangeNames.Ingredients);

            eventBus.Subscribe<RecipeGetByIdEventHandler, RecipeGetByIdDto>(queueNames.Recipes.Get, exchangeNames.Recipes);
            eventBus.Subscribe<RecipesGetByParentIdEventHandler, RecipesGetByParentIdDto>(queueNames.Recipes.GetByParentId, exchangeNames.Recipes); 
            eventBus.Subscribe<RecipesGetPageEventHandler, RecipesGetPageDto>(queueNames.Recipes.GetPage, exchangeNames.Recipes);
            eventBus.Subscribe<RecipesGetAllEventHandler, RecipesGetAllDto>(queueNames.Recipes.GetAll, exchangeNames.Recipes);
            eventBus.Subscribe<RecipesGetCountEventHandler, RecipesGetCountDto>(queueNames.Recipes.Count, exchangeNames.Recipes);
            eventBus.Subscribe<RecipeCreateEventHandler, RecipeCreateDto>(queueNames.Recipes.Create, exchangeNames.Recipes);
            eventBus.Subscribe<RecipeUpdateEventHandler, RecipeUpdateDto>(queueNames.Recipes.Update, exchangeNames.Recipes);
            eventBus.Subscribe<RecipeDeleteEventHandler, RecipeDeleteDto>(queueNames.Recipes.Delete, exchangeNames.Recipes);

            eventBus.Subscribe<WarehouseGetByIdEventHandler, WarehouseGetByIdDto>(queueNames.Warehouses.Get, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehouseGetByParentIdEventHandler, WarehouseGetByParentIdDto>(queueNames.Warehouses.GetByParentId, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehousesGetPageEventHandler, WarehousesGetPageDto>(queueNames.Warehouses.GetPage, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehousesGetAllEventHandler, WarehousesGetAllDto>(queueNames.Warehouses.GetAll, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehousesGetCountEventHandler, WarehousesGetCountDto>(queueNames.Warehouses.Count, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehouseCreateEventHandler, WarehouseCreateDto>(queueNames.Warehouses.Create, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehouseUpdateEventHandler, WarehouseUpdateDto>(queueNames.Warehouses.Update, exchangeNames.Warehouses);
            eventBus.Subscribe<WarehouseDeleteEventHandler, WarehouseDeleteDto>(queueNames.Warehouses.Delete, exchangeNames.Warehouses);

            return app;
        }
    }
}
