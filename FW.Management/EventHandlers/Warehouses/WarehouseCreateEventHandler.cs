using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehouseCreateEventHandler : IIntegrationEventHandler<WarehouseCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehouseCreateEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehouseCreateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var warehouseId = await _warehousesService.Create(msgContext.Message);

            if (warehouseId != Guid.Empty)
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = warehouseId,
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
