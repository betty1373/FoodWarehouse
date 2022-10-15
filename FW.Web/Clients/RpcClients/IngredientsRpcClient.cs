using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.EventBus.Interfaces;
using System.Text.Json;
using FW.Web.RpcClients.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.RabbitMQOptions;

namespace FW.Web.RpcClients
{
    public class IngredientsRpcClient : RpcClientBase, IIngredientsRpcClient
    {
        private readonly IMapper _mapper;
        private readonly string _exchangeName;
        private readonly QueueNames _queueNames;

        public IngredientsRpcClient(IMapper mapper, IConnectionRabbitMQ connection, IConfiguration configuration) :
            base(connection, configuration)
        {
            _mapper = mapper;

            var exchangeNames = configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            _exchangeName = exchangeNames.Ingredients;

            var queueNames = configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();
            _queueNames = queueNames.Ingredients;

            Parallel.ForEach(_queueNames.AllNames, qName =>
                ConfigureRpcClient(_exchangeName, qName));
        }

        public async Task<IngredientResponseVM> Get(Guid id)
        {
            var queryDto = new IngredientGetByIdDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Get, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<IngredientResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<IngredientResponseVM>(responseDto);
            return responseVM;
        }

        public async Task<IEnumerable<IngredientResponseVM>> GetPage(int skip, int take)
        {
            var queryDto = new IngredientsGetPageDto { Skip = skip, Take = take };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetPage, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<IngredientsResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<IngredientResponseVM>>(responseDto?.Ingredients);
            return items;
        }

        public async Task<IEnumerable<IngredientResponseVM>> GetAll()
        {
            var queryDto = new IngredientsGetAllDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetAll, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<IngredientsResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<IngredientResponseVM>>(responseDto?.Ingredients);
            return items;
        }

        public async Task<int> Count()
        {
            var queryDto = new IngredientsGetCountDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Count, queryJsonDto);
            return JsonSerializer.Deserialize<int>(responseJsonDto);
        }

        public async Task<ResponseStatusResult> Create(IngredientVM ingredient, Guid userId)
        {
            var queryDto = _mapper.Map<IngredientCreateDto>(ingredient);
            queryDto.UserId = userId;
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Create, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Update(Guid id, IngredientVM ingredient, Guid userId)
        {
            var queryDto = _mapper.Map<IngredientUpdateDto>(ingredient);
            queryDto.Id = id;
            queryDto.UserId = userId;
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Update, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new IngredientDeleteDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Delete, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }
    }
}
