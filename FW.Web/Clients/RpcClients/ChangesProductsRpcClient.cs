using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.EventBus.Interfaces;
using System.Text.Json;
using FW.Web.RpcClients.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.RabbitMQOptions;
using FW.BusinessLogic.Contracts.Recipes;

namespace FW.Web.RpcClients
{
    public class ChangesProductsRpcClient : RpcClientBase, IChangesProductsRpcClient
    {
        private readonly IMapper _mapper;
        private readonly string _exchangeName;
        private readonly QueueNamesWithGetByParentId _queueNames;

        public ChangesProductsRpcClient(IMapper mapper, IConnectionRabbitMQ connection, IConfiguration configuration) :
            base(connection, configuration)
        {
            _mapper = mapper;

            var exchangeNames = configuration.GetSection(RabbitMqExchangeNamesOptions.KeyValue).Get<RabbitMqExchangeNamesOptions>();
            _exchangeName = exchangeNames.ChangesProducts;

            var queueNames = configuration.GetSection(RabbitMqQueueNamesOptions.KeyValue).Get<RabbitMqQueueNamesOptions>();
            _queueNames = queueNames.ChangesProducts;

            foreach (string qName in _queueNames.AllNames)
                ConfigureRpcClient(_exchangeName, qName);
        }
        public async Task<IEnumerable<ChangesProductResponseVM>> GetByParentId(Guid ParentId)
        {
            var queryDto = new ChangesProductsGetByParentIdDto { ProductId = ParentId };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetByParentId, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ChangesProductsResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<ChangesProductResponseVM>>(responseDto?.ChangesProducts);
            return items;
        }
        public async Task<int> Count()
        {
            var queryDto = new ChangesProductsGetCountDto();
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Count, queryJsonDto);
            return JsonSerializer.Deserialize<int>(responseJsonDto);
        }

        public async Task<ResponseStatusResult> Create(ChangesProductVM changesProduct, Guid userId)
        {
            var queryDto = _mapper.Map<ChangesProductCreateDto>(changesProduct);
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Create, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Update(Guid id, ChangesProductVM changesProduct, Guid userId)
        {
            var queryDto = _mapper.Map<ChangesProductUpdateDto>(changesProduct);
            queryDto.Id = id;
            queryDto.UserId = userId;

            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Update, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<ResponseStatusResult> Delete(Guid id)
        {
            var queryDto = new ChangesProductDeleteDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Delete, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ResponseStatusResult>(responseJsonDto);
            return responseDto;
        }

        public async Task<IEnumerable<ChangesProductResponseVM>> GetAll()
        {
            var queryDto = new ChangesProductsGetAllDto {};
            var queryJsonDto = JsonSerializer.Serialize(queryDto);
            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetAll, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ChangesProductsResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<ChangesProductResponseVM>>(responseDto?.ChangesProducts);
            return items;
        }

        public async Task<ChangesProductResponseVM> Get(Guid id)
        {
            var queryDto = new ChangesProductGetByIdDto { Id = id };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.Get, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ChangesProductResponseDto>(responseJsonDto);

            var responseVM = _mapper.Map<ChangesProductResponseVM>(responseDto);
            return responseVM;
        }

        public async Task<IEnumerable<ChangesProductResponseVM>> GetPage(int skip, int take)
        {
            var queryDto = new ChangesProductGetPageDto { Skip = skip, Take = take };
            var queryJsonDto = JsonSerializer.Serialize(queryDto);

            var responseJsonDto = await CallAsync(_exchangeName, _queueNames.GetPage, queryJsonDto);
            var responseDto = JsonSerializer.Deserialize<ChangesProductsResponseDto>(responseJsonDto);

            var items = _mapper.Map<List<ChangesProductResponseVM>>(responseDto?.ChangesProducts);
            return items;
        }

    }
}
