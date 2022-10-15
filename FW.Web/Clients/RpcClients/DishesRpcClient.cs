using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.EventBus.Interfaces;
using System.Text.Json;
using FW.Web.RpcClients.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.RabbitMQOptions;
using FW.BusinessLogic.Contracts.Warehouses;

namespace FW.Web.RpcClients
{
    public class DishesRpcClient : RpcClientBase, IDishesRpcClient
    {
        private readonly IMapper _mapper;
        private readonly string _exchangeName;
        private readonly QueueNamesDishes _queueNames;

        public DishesRpcClient(IMapper mapper, IConnectionRabbitMQ connection, IConfiguration configuration) :
            base(connection, configuration)
        {
            _mapper = mapper;

            var exchangeNames = configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            _exchangeName = exchangeNames.Dishes;

            var queueNames = configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();
            _queueNames = queueNames.Dishes;

            Parallel.ForEach(_queueNames.AllNames, qName =>
                ConfigureRpcClient(_exchangeName, qName));
        }

        public async Task<int> Count()
        {
            var queryDto = new DishesGetCountDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Count, queryJsonDto);
            return JsonSerializer.Deserialize<int>(responseJsonDto);
        }
        public async Task<IEnumerable<DishResponseVM>> GetAll()
        {
            var queryDto = new DishesGetAllDto { };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetAll, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<DishesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<DishResponseVM>>(responseDto?.Dishes);
            return items;
        }

        public async Task<IEnumerable<DishResponseVM>> GetByParentId(Guid ParentId)
        {
            var queryDto = new DishesGetByParentIdDto { UserId = ParentId };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetByParentId, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<DishesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<DishResponseVM>>(responseDto?.Dishes);
            return items;
        }
        public async Task<DishResponseVM> Get(Guid id)
        {
            var queryDto = new DishGetByIdDto { Id = id};
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Get, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<DishResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<DishResponseVM>(responseDto);
            return responseVM;
        }

        public async Task<IEnumerable<DishResponseVM>> GetPage(int skip, int take)
        {
            var queryDto = new DishesGetPageDto { Skip = skip, Take = take };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetPage, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<DishesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<DishResponseVM>>(responseDto?.Dishes);
            return items;
        }

        public async Task<ResponseStatusResult> Create(DishVM dish, Guid userId)
        {
            var queryDto = _mapper.Map<DishCreateDto>(dish);
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Create, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }

        public async Task<ResponseStatusResult> Update(Guid id, DishVM dish, Guid userId)
        {
            var queryDto = _mapper.Map<DishUpdateDto>(dish);
            queryDto.Id = id;
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Update, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new DishDeleteDto { Id = id };

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Delete, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }
        public async Task<ResponseStatusResult> Cook(Guid id, Guid userId, int numPortions)
        {
            var queryDto = new DishCookDto { Id = id, UserId = userId, NumPortions = numPortions };

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Cook, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);

            return responseDto;
        }
    }
}
