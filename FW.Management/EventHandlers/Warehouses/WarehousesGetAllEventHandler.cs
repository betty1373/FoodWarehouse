using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.EventHandlers.Warehouses
{
    public class WarehousesGetAllEventHandler : IIntegrationEventHandler<WarehousesGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IWarehousesService _warehousesService;

        public WarehousesGetAllEventHandler(ILogger logger, IWarehousesService warehousesService)
        {
            _logger = logger;
            _warehousesService = warehousesService;
        }

        public async Task Handle(IntegrationContext<WarehousesGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var warehousesDto = await _warehousesService.GetAll();

            await msgContext.RespondAsync(new WarehousesResponseDto 
            {
                Warehouses = warehousesDto
            });
        }
    }
}
