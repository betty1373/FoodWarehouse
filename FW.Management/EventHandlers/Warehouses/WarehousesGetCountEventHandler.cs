using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts.Warehouses;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehousesGetCountEventHandler : IIntegrationEventHandler<WarehousesGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehousesGetCountEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehousesGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var count = await _warehousesService.Count();

            await msgContext.RespondAsync(count);
        }
    }
}
