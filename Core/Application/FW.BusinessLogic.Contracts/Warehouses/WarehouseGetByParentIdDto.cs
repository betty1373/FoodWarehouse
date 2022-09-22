using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Warehouses
{
    public class WarehouseGetByParentIdDto : IIntegrationEvent
    {
        public Guid UserId { get; set; }
    }
}
