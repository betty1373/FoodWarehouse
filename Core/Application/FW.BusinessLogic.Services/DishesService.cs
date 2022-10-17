using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FW.ResponseStatus;

namespace FW.BusinessLogic.Services;

public class DishesService : IDishesService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICookingDishService _cookingDishService;
    public DishesService(ApplicationContext dbContext, IMapper mapper, ICookingDishService cookingDishService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cookingDishService = cookingDishService;
    }
    public async Task<IEnumerable<DishResponseDto>> GetAll()
    {
        var items = await _dbContext.Dishes.ToListAsync();
        return _mapper.Map<List<DishResponseDto>>(items);
    }
    public async Task<int> Count()
    {
        var count = await _dbContext.Dishes.CountAsync();
        return count;
    }
    public async Task<IEnumerable<DishResponseDto>> GetPaged(int Skip, int Take)
    {
        var items = await _dbContext.Dishes.Skip(Skip).Take(Take).ToListAsync();
        return _mapper.Map<List<DishResponseDto>>(items);
    }
    public async Task<IEnumerable<DishResponseDto>> GetByParentId(Guid ParentId)
    {
        var items = await _dbContext.Dishes.Where(u => EF.Property<Guid?>(u, "UserId").Equals(ParentId)).ToListAsync();

        return _mapper.Map<List<DishResponseDto>>(items);
    }
    public async Task<DishResponseDto> GetById(Guid id)
    {
        var dish = await _dbContext.Dishes.FindAsync(id);
        return _mapper.Map<DishResponseDto>(dish);
    }

    public async Task<Guid> Create(DishCreateDto dto)
    {
        var item = _mapper.Map<Dishes>(dto);
 
        await _dbContext.Dishes.AddAsync(item);
        await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);
        return item.Id;
    }

    public async Task<bool> Delete(Guid id)
    {
        var item = await _dbContext.Dishes.FindAsync(id);
        if (item != null)
        {
            _dbContext.Dishes.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> Update(DishUpdateDto dto)
    {
        var item = _mapper.Map<Dishes>(dto);
        var entity = await _dbContext.Dishes.FindAsync(item.Id);
        if (entity != null)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);

            return true;
        }

        return false;
    }
    public async Task<ResponseStatusResult> Cook(Guid dishId, Guid userId, int numPortions)
    {
        return await _cookingDishService.Cook(dishId, userId, numPortions);
    }
}
