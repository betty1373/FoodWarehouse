using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductDeleteEventHandler : IIntegrationEventHandler<ChangesProductDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductDeleteEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var changesProductId = msgContext.Message.Id;
            var status = await _changesProductsService.Delete(changesProductId);
            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = changesProductId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = changesProductId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
