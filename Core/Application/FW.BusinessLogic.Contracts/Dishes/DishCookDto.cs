using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Dishes
{
    public class DishCookDto : IIntegrationEvent
    {
        public Guid Id { get; set; }

        public Guid WarehouseId { get; set; }

        public int NumPortions { get; set; }
    }
}
