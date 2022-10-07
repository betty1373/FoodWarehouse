using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CategoriesService(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CategoryResponseDto>> GetAll()
    {
        var items = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<CategoryResponseDto>>(items);
    }
    public async Task<int> Count()
    {
        var count = await _dbContext.Categories.CountAsync();
        return count;
    }
    public async Task<IEnumerable<CategoryResponseDto>> GetPaged(int Skip, int Take)
    {
           
        var items = await _dbContext.Categories.Skip(Skip).Take(Take).ToListAsync();
        return _mapper.Map<List<CategoryResponseDto>>(items);
    }

    public async Task<CategoryResponseDto> GetById(Guid id)
    {
        var item = await _dbContext.Categories.FindAsync(id);
        return _mapper.Map<CategoryResponseDto>(item);
    }

    public async Task<Guid> Create(CategoryCreateDto dto)
    {
        var item = _mapper.Map<Categories>(dto);
      
        await _dbContext.Categories.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        return item.Id;
    }

    public async Task<bool> Delete(Guid id)
    {
        var item = await _dbContext.Categories.FindAsync(id);
        if (item != null)
        {
            _dbContext.Categories.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> Update(CategoryUpdateDto dto)
    {
        var item = _mapper.Map<Categories>(dto);
        var entity = await _dbContext.Categories.FindAsync(item.Id);
        if (entity != null)
        {
         
            _dbContext.Entry(entity).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
