using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehouseDeleteEventHandler : IIntegrationEventHandler<WarehouseDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehouseDeleteEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehouseDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var warehouseId = msgContext.Message.Id;
            var status = await _warehousesService.Delete(warehouseId);
            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = warehouseId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = warehouseId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
