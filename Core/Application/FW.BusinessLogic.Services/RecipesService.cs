using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services;

public class RecipesService : IRecipesService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public RecipesService(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RecipeResponseDto>> GetAll()
    {
       // var items = await _dbContext.Recipes.Include(x => x.Ingredients).ThenInclude(ing => ing.Name).ToListAsync();
  //      var items = await _dbContext.Recipes.Include(x => x.Ingredients).ToListAsync();
        var items = await _dbContext.Recipes.ToListAsync();
        return _mapper.Map<List<RecipeResponseDto>>(items);
    } 
    public async Task<int> Count()
    {
        var count = await _dbContext.Recipes.CountAsync();
        return count;
    }
    public async Task<IEnumerable<RecipeResponseDto>> GetPaged(int Skip, int Take)
    {
        var items = await _dbContext.Recipes.Include(x => x.Ingredients).Skip(Skip).Take(Take).ToListAsync();
        return _mapper.Map<List<RecipeResponseDto>>(items);
    }

    public async Task<RecipeResponseDto> GetById(Guid id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    public async Task<IEnumerable<RecipeResponseDto>> GetByParentId(Guid ParentId)
    {
        var items = await _dbContext.Recipes.Include(x => x.Ingredients).Where(p=>p.DishesId == ParentId).ToListAsync();

        return _mapper.Map<List<RecipeResponseDto>>(items);
    }

    public async Task<Guid> Create(RecipeCreateDto dto)
    {
        var item = _mapper.Map<Recipes>(dto);
     
        await _dbContext.Recipes.AddAsync(item );
        await _dbContext.SaveChangesAsync();
        return item.Id;
    }

    public async Task<bool> Delete(Guid id)
    {
        var item = await _dbContext.Recipes.FindAsync(id);
        if (item != null)
        {
            _dbContext.Recipes.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> Update(RecipeUpdateDto dto)
    {
        var item = _mapper.Map<Recipes>(dto);
        var entity = await _dbContext.Recipes.FindAsync(item.Id);
        if (entity != null)
        {
       
            _dbContext.Entry(entity).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
