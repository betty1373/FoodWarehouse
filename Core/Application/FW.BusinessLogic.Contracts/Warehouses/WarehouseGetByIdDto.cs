using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Warehouses
{
    public class WarehouseGetByIdDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
