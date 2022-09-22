using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts.ChangesProducts;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductsGetCountEventHandler : IIntegrationEventHandler<ChangesProductsGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductsGetCountEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductsGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var count = await _changesProductsService.Count();

            await msgContext.RespondAsync(count);
        }
    }
}
