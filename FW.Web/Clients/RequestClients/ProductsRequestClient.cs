using AutoMapper;
using FW.BusinessLogic.Contracts;
using FW.ResponseStatus;
using FW.BusinessLogic.Contracts.Products;
using FW.Web.RequestClients.Interfaces;
using FW.Models;
using MassTransit;

namespace FW.Web.RequestClients
{
    public class ProductsRequestClient : IProductsRequestClient
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRequestClient<ProductCreateDto> _createClient;
        private readonly IRequestClient<ProductGetByIdDto> _getByIdClient;
        private readonly IRequestClient<ProductUpdateDto> _updateClient;
        private readonly IRequestClient<ProductDeleteDto> _deleteClient;
        private readonly IRequestClient<ProductsGetPageDto> _getPageClient;
        private readonly IRequestClient<ProductsGetAllDto> _getAllClient;
        private readonly IRequestClient<ProductsGetByParentIdDto> _getByParentIdClient;
        private readonly IRequestClient<ProductsGetCountDto> _getCountClient;

        public ProductsRequestClient(
            IMapper mapper,
            ILogger logger,
            IRequestClient<ProductCreateDto> createClient,
            IRequestClient<ProductGetByIdDto> getByIdClient,
            IRequestClient<ProductUpdateDto> updateClient,
            IRequestClient<ProductDeleteDto> deleteClient,
            IRequestClient<ProductsGetPageDto> getPageClient,
            IRequestClient<ProductsGetAllDto> getAllClient,
            IRequestClient<ProductsGetCountDto> getCountClient,
            IRequestClient<ProductsGetByParentIdDto> getByParentIdClient)
        {
            _mapper = mapper;
            _logger = logger;
            _getByIdClient = getByIdClient;
            _getPageClient = getPageClient;
            _createClient = createClient;
            _updateClient = updateClient;
            _deleteClient = deleteClient;
            _getAllClient = getAllClient;
            _getCountClient = getCountClient;
            _getByParentIdClient = getByParentIdClient;
        }

        public async Task<ProductResponseVM> Get(Guid id)
        {
            var queryDto = new ProductGetByIdDto { Id = id };
            var responseDto = await _getByIdClient.GetResponse<ProductResponseDto>(queryDto);

            var result = _mapper.Map<ProductResponseVM>(responseDto.Message);
            return result;
        }

        public async Task<IEnumerable<ProductResponseVM>> GetByParentId(Guid parentId)
        {
            var queryDto = new ProductsGetByParentIdDto { WarehauseId = parentId };
            var responseDto = await _getByParentIdClient.GetResponse<ProductsResponseDto>(queryDto);

            var result = _mapper.Map<List<ProductResponseVM>>(responseDto.Message.Products);
            return result;
        }
        public async Task<IEnumerable<ProductResponseVM>> GetPage(int Skip, int Take)
        {
            var queryDto = new ProductsGetPageDto { Skip = Skip, Take = Take };
            var responseDto = await _getPageClient.GetResponse<ProductsResponseDto>(queryDto);

            var result = _mapper.Map<List<ProductResponseVM>>(responseDto.Message.Products);
            return result;
        }
        public async Task<IEnumerable<ProductResponseVM>> GetAll()
        {
            var queryDto = new ProductsGetAllDto { };
            var responseDto = await _getAllClient.GetResponse<ProductsResponseDto>(queryDto);

            var result = _mapper.Map<List<ProductResponseVM>>(responseDto.Message.Products);
            return result;
        }

        public async Task<int> Count()
        {
            var queryDto = new ProductsGetCountDto();
            var responseDto = await _getCountClient.GetResponse<CountResponseDto>(queryDto);

            var result = responseDto.Message;
            return result.Count;
        }

        public async Task<ResponseStatusResult> Create(ProductVM product,Guid userId)
        {
            var queryDto = _mapper.Map<ProductCreateDto>(product);
            queryDto.UserId =userId;
            var responseDto = await _createClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }

        public async Task<ResponseStatusResult> Update(Guid id, ProductVM product, Guid userId)
        {
            var queryDto = _mapper.Map<ProductUpdateDto>(product);
            queryDto.Id = id;
            queryDto.UserId = userId;
            var responseDto = await _updateClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new ProductDeleteDto { Id = id };
            var responseDto = await _deleteClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }
    }
}
