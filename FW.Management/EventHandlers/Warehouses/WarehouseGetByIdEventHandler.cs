using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehouseGetByIdEventHandler : IIntegrationEventHandler<WarehouseGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehouseGetByIdEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehouseGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var warehouseId = msgContext.Message.Id;
            var warehouseDto = await _warehousesService.GetById(warehouseId);

            if (warehouseDto != null)
                await msgContext.RespondAsync(warehouseDto);
            else
                await msgContext.RespondAsync(new WarehouseResponseDto {});
        }
    }
}
