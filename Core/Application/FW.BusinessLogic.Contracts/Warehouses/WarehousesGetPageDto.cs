using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Warehouses
{
    public class WarehousesGetPageDto : IIntegrationEvent
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
