using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Dishes
{
    public class DishDeleteDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
