using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services;

public class IngredientsService : IIngredientsService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public IngredientsService(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<IngredientResponseDto>> GetAll()
    {
        var items = await _dbContext.Ingredients.ToListAsync();
        return _mapper.Map<List<IngredientResponseDto>>(items);
    }
    public async Task<int> Count()
    {
        var count = await _dbContext.Ingredients.CountAsync();
        return count;
    }
    public async Task<IEnumerable<IngredientResponseDto>> GetPaged(int Skip, int Take)
    {
        var items = await _dbContext.Ingredients.Skip(Skip).Take(Take).ToListAsync();
        return _mapper.Map<List<IngredientResponseDto>>(items);
    }

    public async Task<IngredientResponseDto> GetById(Guid id)
    {
        var item = await _dbContext.Ingredients.FindAsync(id);
        return _mapper.Map<IngredientResponseDto>(item);
    }

    public async Task<Guid> Create(IngredientCreateDto dto)
    {
        var item = _mapper.Map<Ingredients>(dto);
     
        await _dbContext.Ingredients.AddAsync(item);
        await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);
        return item.Id;
    }

    public async Task<bool> Delete(Guid id)
    {
        var item = await _dbContext.Ingredients.FindAsync(id);
        if (item != null)
        {
            _dbContext.Ingredients.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> Update(IngredientUpdateDto dto)
    {
        var item = _mapper.Map<Ingredients>(dto);
        var entity = await _dbContext.Ingredients.FindAsync(item.Id);
        if (entity != null)
        {
        
            _dbContext.Entry(entity).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);

            return true;
        }

        return false;
    }
}
