using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services
{
    public class WarehousesService : IWarehousesService
    {
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public WarehousesService(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<WarehouseResponseDto>> GetAll()
        {
            var items = await _dbContext.Warehouses.ToListAsync();
            return _mapper.Map<List<WarehouseResponseDto>>(items);
        }
        public async Task<int> Count()
        {
            var count = await _dbContext.Warehouses.CountAsync();
            return count;
        }
        public async Task<IEnumerable<WarehouseResponseDto>> GetPaged(int Skip, int Take)
        {
            var items = await _dbContext.Warehouses.Skip(Skip).Take(Take).ToListAsync();
            return _mapper.Map<List<WarehouseResponseDto>>(items);
        }

        public async Task<WarehouseResponseDto> GetById(Guid id)
        {
            var item = await _dbContext.Warehouses.FindAsync(id);
            return _mapper.Map<WarehouseResponseDto>(item);
        }

        public async Task<Guid> Create(WarehouseCreateDto dto)
        {
            var item = _mapper.Map<Warehouses>(dto);
          
            await _dbContext.Warehouses.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var item = await _dbContext.Warehouses.FindAsync(id);
            if (item != null)
            {
                _dbContext.Warehouses.Remove(item);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Update(WarehouseUpdateDto dto)
        {
            var item = _mapper.Map<Warehouses>(dto);
            var entity = await _dbContext.Warehouses.FindAsync(item.Id);
            if (entity != null)
            {
            
                _dbContext.Entry(entity).CurrentValues.SetValues(item);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<WarehouseResponseDto> GetByParentId(Guid ParentId)
        {
            var item = await _dbContext.Warehouses.Where(u => EF.Property<Guid?>(u, "UserId") == ParentId).FirstOrDefaultAsync();
            var dto = _mapper.Map<WarehouseResponseDto>(item);
            return dto;
        }
    }
}
