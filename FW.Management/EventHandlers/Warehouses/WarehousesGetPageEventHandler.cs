using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehousesGetPageEventHandler : IIntegrationEventHandler<WarehousesGetPageDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehousesGetPageEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehousesGetPageDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            int page = msgContext.Message.Skip;
            int pageSize = msgContext.Message.Take;
            var warehousesDto = await _warehousesService.GetPaged(page, pageSize);

            await msgContext.RespondAsync(new WarehousesResponseDto 
            {
                Warehouses = warehousesDto
            });
        }
    }
}
