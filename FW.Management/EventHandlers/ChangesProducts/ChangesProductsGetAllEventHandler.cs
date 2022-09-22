using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductsGetAllEventHandler : IIntegrationEventHandler<ChangesProductsGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IChangesProductsService _changesProductsService;

        public ChangesProductsGetAllEventHandler(ILogger logger, IChangesProductsService changesProductsService)
        {
            _logger = logger;
            _changesProductsService = changesProductsService;
        }

        public async Task Handle(IntegrationContext<ChangesProductsGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var changesProductsDto = await _changesProductsService.GetAll();

            await msgContext.RespondAsync(new ChangesProductsResponseDto 
            {
                ChangesProducts = changesProductsDto
            });
        }
    }
}