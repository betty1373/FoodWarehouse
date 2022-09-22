using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductGetByIdEventHandler : IIntegrationEventHandler<ChangesProductGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductGetByIdEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var changesProductId = msgContext.Message.Id;
            var changesProductDto = await _changesProductsService.GetById(changesProductId);

            if (changesProductDto != null)
                await msgContext.RespondAsync(changesProductDto);
            else
                await msgContext.RespondAsync(new ChangesProductResponseDto{});
        }
    }
}
