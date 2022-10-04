using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.EventBus.Interfaces;
using FW.RabbitMQOptions;
using System.Text.Json;
using FW.Web.RpcClients.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.BusinessLogic.Contracts.Warehouses;

namespace FW.Web.RpcClients
{
    public class RecipesRpcClient : RpcClientBase, IRecipesRpcClient
    {
        private readonly IMapper _mapper;
        private readonly string _exchangeName;
        private readonly QueueNamesWithGetByParentId _queueNames;

        public RecipesRpcClient(IMapper mapper, IConnectionRabbitMQ connection, IConfiguration configuration) :
            base(connection, configuration)
        {
            _mapper = mapper;

            var exchangeNames = configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            _exchangeName = exchangeNames.Recipes;

            var queueNames = configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();
            _queueNames = queueNames.Recipes;

            foreach (string qName in _queueNames.AllNames)
                ConfigureRpcClient(_exchangeName, qName);
        }
        public async Task<RecipeResponseVM> Get(Guid id)
        {
            var queryDto = new RecipeGetByIdDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Get, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<RecipeResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<RecipeResponseVM>(responseDto);
            return responseVM;
        }
        public async Task<IEnumerable<RecipeResponseVM>> GetByParentId(Guid ParentId)
        {
            var queryDto = new  RecipesGetByParentIdDto{ DishId = ParentId };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetByParentId, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<RecipesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<RecipeResponseVM>>(responseDto?.Recipes);
            return items;
        }

        public async Task<IEnumerable<RecipeResponseVM>> GetPage(int Skip, int Take)
        {
            var queryDto = new RecipesGetPageDto { Skip = Skip, Take = Take };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetPage, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<RecipesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<RecipeResponseVM>>(responseDto?.Recipes);
            return items;
        }
        public async Task<IEnumerable<RecipeResponseVM>> GetAll()
        {
            var queryDto = new RecipesGetAllDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetAll, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<RecipesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<RecipeResponseVM>>(responseDto?.Recipes);
            return items;
        }

        public async Task<int> Count()
        {
            var queryDto = new RecipesGetCountDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Count, queryJsonDto);
            return JsonSerializer.Deserialize<int>(responseJsonDto);
        }
        public async Task<ResponseStatusResult> Create(RecipeVM recipe, Guid userId)
        {
            var queryDto = _mapper.Map<RecipeCreateDto>(recipe);
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Create, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }

        public async Task<ResponseStatusResult> Update(Guid id, RecipeVM recipe, Guid userId)
        {
            var queryDto = _mapper.Map<RecipeUpdateDto>(recipe);
            queryDto.Id = id;
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Update, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new RecipeDeleteDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Delete, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }
    }
}
