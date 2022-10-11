using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductsGetByParentIdEventHandler : IIntegrationEventHandler<ChangesProductsGetByParentIdDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductsGetByParentIdEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductsGetByParentIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var parentId = msgContext.Message.ProductId;
            var itemsDto = await _changesProductsService.GetByParentId(parentId);

            await msgContext.RespondAsync(new ChangesProductsResponseDto { ChangesProducts = itemsDto });
        }
    }
}
