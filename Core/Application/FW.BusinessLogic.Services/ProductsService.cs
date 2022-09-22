using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;
using FW.Domain;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services
{
    public  class ProductsService : IProductsService
    {
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public ProductsService(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAll()
        {
            var items = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductResponseDto>>(items);
        }
        public async Task<int> Count()
        {
            var count = await _dbContext.Products.CountAsync();
            return count;
        }
        public async Task<IEnumerable<ProductResponseDto>> GetPaged(int Skip, int Take)
        {
            var items = await _dbContext.Products.Skip(Skip).Take(Take).ToListAsync();
            return _mapper.Map<List<ProductResponseDto>>(items);
        }

        public async Task<ProductResponseDto> GetById(Guid id)
        {
            var item = await _dbContext.Products.FindAsync(id);
            return _mapper.Map<ProductResponseDto>(item);
        }

        public async Task<Guid> Create(ProductCreateDto dto)
        {
            var item = _mapper.Map<Products>(dto);
         
            await _dbContext.Products.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var item = await _dbContext.Products.FindAsync(id);
            if (item != null)
            {
                _dbContext.Products.Remove(item);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Update(ProductUpdateDto dto)
        {
            var item = _mapper.Map<Products>(dto);
            var entity = await _dbContext.Products.FindAsync(item.Id);
            if (entity != null)
            {
              
                _dbContext.Entry(entity).CurrentValues.SetValues(item);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
