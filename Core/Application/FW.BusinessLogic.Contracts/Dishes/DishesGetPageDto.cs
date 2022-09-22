using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Dishes
{
    public class DishesGetPageDto : IIntegrationEvent
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
