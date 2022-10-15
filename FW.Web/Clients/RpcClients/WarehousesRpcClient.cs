using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.EventBus.Interfaces;
using System.Text.Json;
using FW.Web.RpcClients.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.Models;
using FW.RabbitMQOptions;

namespace FW.Web.RpcClients
{
    public class WarehousesRpcClient : RpcClientBase, IWarehousesRpcClient
    {
        private readonly IMapper _mapper;
        private readonly string _exchangeName;
        private readonly QueueNamesWithGetByParentId _queueNames;

        public WarehousesRpcClient(IMapper mapper, IConnectionRabbitMQ connection, IConfiguration configuration) :
            base(connection, configuration)
        {
            _mapper = mapper;

            var exchangeNames = configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            _exchangeName = exchangeNames.Warehouses;

            var queueNames = configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();
            _queueNames = queueNames.Warehouses;

            Parallel.ForEach(_queueNames.AllNames, qName =>
                 ConfigureRpcClient(_exchangeName, qName));
        }

        public async Task<WarehouseResponseVM> Get(Guid id)
        {
            var queryDto = new WarehouseGetByIdDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Get, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<WarehouseResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<WarehouseResponseVM>(responseDto);
            return responseVM;
        }

        public async Task<WarehouseResponseVM> GetByParentId(Guid ParentId)
        {
            var queryDto = new WarehouseGetByParentIdDto { UserId = ParentId };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetByParentId, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<WarehouseResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<WarehouseResponseVM>(responseDto);
            return responseVM;
        }

        public async Task<IEnumerable<WarehouseResponseVM>> GetPage(int Skip, int Take)
        {
            var queryDto = new WarehousesGetPageDto { Skip = Skip, Take = Take };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetPage, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<WarehousesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<WarehouseResponseVM>>(responseDto?.Warehouses);
            return items;
        }

        public async Task<IEnumerable<WarehouseResponseVM>> GetAll()
        {
            var queryDto = new WarehousesGetAllDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetAll, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<WarehousesResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<WarehouseResponseVM>>(responseDto?.Warehouses);
            return items;
        }

        public async Task<int> Count()
        {
            var queryDto = new WarehousesGetCountDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Count, queryJsonDto);
            return JsonSerializer.Deserialize<int>(responseJsonDto);
        }

        public async Task<ResponseStatusResult> Create(WarehouseVM warehouse, Guid userId)
        {
            var queryDto = _mapper.Map<WarehouseCreateDto>(warehouse);
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Create, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Update(Guid id, WarehouseVM warehouse, Guid userId)
        {
            var queryDto = _mapper.Map<WarehouseUpdateDto>(warehouse);
            queryDto.Id = id;
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Update, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new WarehouseDeleteDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Delete, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }
    }
}
