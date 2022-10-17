using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services;

public class ChangesProductsService : IChangesProductsService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public ChangesProductsService(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ChangesProductResponseDto>> GetAll()
    {
        var items = await _dbContext.ChangesProducts.ToListAsync();
        var response = new List<ChangesProductResponseDto>();
        foreach (var item in items) 
        {
            response.Add(_mapper.Map<ChangesProductResponseDto>(item));
        }
        return _mapper.Map<List<ChangesProductResponseDto>>(items);
    }
    public async Task<int> Count()
    {
        var count = await _dbContext.ChangesProducts.CountAsync();
        return count;
    }
    public async Task<IEnumerable<ChangesProductResponseDto>> GetByParentId(Guid ParentId)
    {
        var items = await _dbContext.ChangesProducts.Where(p => p.ProductId.Equals(ParentId)).ToListAsync();
     
        return _mapper.Map<List<ChangesProductResponseDto>>(items);
    }
    public async Task<IEnumerable<ChangesProductResponseDto>> GetPaged(int Skip, int Take)
    {
        var items = await _dbContext.ChangesProducts.Skip(Skip).Take(Take).ToListAsync();
        return _mapper.Map<List<ChangesProductResponseDto>>(items);
    }

    public async Task<ChangesProductResponseDto> GetById(Guid id)
    {
        var item = await _dbContext.ChangesProducts.FindAsync(id);
        return _mapper.Map<ChangesProductResponseDto>(item);
    }

    public async Task<Guid> Create(ChangesProductCreateDto dto)
    {
        var item = _mapper.Map<ChangesProducts>(dto);
     
        await _dbContext.ChangesProducts.AddAsync(item);
        await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);
        return item.Id;
    }

    public async Task<bool> Delete(Guid id)
    {
        var item = await _dbContext.ChangesProducts.FindAsync(id);
        if (item != null)
        {
            _dbContext.ChangesProducts.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> Update(ChangesProductUpdateDto dto)
    {
        var item = _mapper.Map<ChangesProducts>(dto);
        var entity = await _dbContext.ChangesProducts.FindAsync(item.Id);
        if (entity != null)
        {
       
            _dbContext.Entry(entity).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesWithUserIdAsync(dto.UserId);

            return true;
        }

        return false;
    }
}
