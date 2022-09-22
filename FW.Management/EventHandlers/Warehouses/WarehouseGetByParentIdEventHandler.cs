using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehouseGetByParentIdEventHandler : IIntegrationEventHandler<WarehouseGetByParentIdDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehouseGetByParentIdEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehouseGetByParentIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var userId = msgContext.Message.UserId;
            var warehouseDto = await _warehousesService.GetByParentId(userId);

            if (warehouseDto != null)
                await msgContext.RespondAsync(warehouseDto);
            else
                await msgContext.RespondAsync(new WarehouseResponseDto {});
        }
    }
}
