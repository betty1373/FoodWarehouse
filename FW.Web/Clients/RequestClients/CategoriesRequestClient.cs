using AutoMapper;
using FW.ResponseStatus;
using FW.BusinessLogic.Contracts.Category;
using FW.Web.RequestClients.Interfaces;
using FW.Models;
using MassTransit;
using FW.BusinessLogic.Contracts;


namespace FW.Web.RequestClients
{
    public class CategoriesRequestClient : ICategoriesRequestClient
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRequestClient<CategoryCreateDto> _createClient;
        private readonly IRequestClient<CategoryGetByIdDto> _getByIdClient;
        private readonly IRequestClient<CategoryUpdateDto> _updateClient;
        private readonly IRequestClient<CategoryDeleteDto> _deleteClient;
        private readonly IRequestClient<CategoriesGetPageDto> _getPageClient;
        private readonly IRequestClient<CategoriesGetAllDto> _getAllClient;
        private readonly IRequestClient<CategoriesGetCountDto> _getCountClient;

        public CategoriesRequestClient(
            IMapper mapper,
            ILogger logger,
            IRequestClient<CategoryCreateDto> createClient,
            IRequestClient<CategoryGetByIdDto> getByIdClient,
            IRequestClient<CategoryUpdateDto> updateClient,
            IRequestClient<CategoryDeleteDto> deleteClient,
            IRequestClient<CategoriesGetPageDto> getPageClient,
            IRequestClient<CategoriesGetAllDto> getAllClient,
            IRequestClient<CategoriesGetCountDto> getCountClient)
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
        }

        public async Task<CategoryResponseVM> Get(Guid id)
        {
            var queryDto = new CategoryGetByIdDto { Id = id };
            var responseDto = await _getByIdClient.GetResponse<CategoryResponseDto>(queryDto);

            var result = _mapper.Map<CategoryResponseVM>(responseDto.Message);
            return result;
        }

        public async Task<IEnumerable<CategoryResponseVM>> GetPage(int Skip, int Take)
        {
            var queryDto = new CategoriesGetPageDto { Skip =Skip, Take = Take };
            var responseDto = await _getPageClient.GetResponse<CategoriesResponseDto>(queryDto);

            var result = _mapper.Map<List<CategoryResponseVM>>(responseDto.Message.Categories);
            return result;
        }

        public async Task<IEnumerable<CategoryResponseVM>> GetAll()
        {
            var queryDto = new CategoriesGetAllDto { };
            var responseDto = await _getAllClient.GetResponse<CategoriesResponseDto>(queryDto);

            var result = _mapper.Map<List<CategoryResponseVM>>(responseDto.Message.Categories);
            return result;
        }

        public async Task<int> Count()
        {
            var queryDto = new CategoriesGetCountDto();
            var responseDto = await _getCountClient.GetResponse<CountResponseDto>(queryDto);

            var result = responseDto.Message;
            return result.Count;
        }

        public async Task<ResponseStatusResult> Create(CategoryVM category, Guid userId)
        {
            var queryDto = _mapper.Map<CategoryCreateDto>(category);
            queryDto.UserId = userId;
            var responseDto = await _createClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }

        public async Task<ResponseStatusResult> Update(Guid id, CategoryVM category, Guid userId)
        {
            var queryDto = _mapper.Map<CategoryUpdateDto>(category);
            queryDto.Id = id;
            queryDto.UserId = userId;
            var responseDto = await _updateClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new CategoryDeleteDto { Id = id };
            var responseDto = await _deleteClient.GetResponse<ResponseStatusResult>(queryDto);

            var result = responseDto.Message;
            return result;
        }
    }
}
