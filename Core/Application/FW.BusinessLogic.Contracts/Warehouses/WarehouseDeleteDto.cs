using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Warehouses
{
    public class WarehouseDeleteDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
