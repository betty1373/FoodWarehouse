using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductCreateEventHandler : IIntegrationEventHandler<ChangesProductCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductCreateEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductCreateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var id = await _changesProductsService.Create(msgContext.Message);

            if (id != Guid.Empty)
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = id,
                    Status = StatusResult.Ok,
                    Title = "Added"
                });
            else
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.Error,
                    Title = "Error"
                });
        }
    }
}
