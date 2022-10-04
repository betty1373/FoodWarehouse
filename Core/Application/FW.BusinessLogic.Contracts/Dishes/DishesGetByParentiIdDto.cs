using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Dishes
{
    public class DishesGetByParentIdDto : IIntegrationEvent
    {
        public Guid UserId { get; set; }
    }
}
