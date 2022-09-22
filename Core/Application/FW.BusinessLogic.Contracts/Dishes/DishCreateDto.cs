using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Dishes
{
    public class DishCreateDto : IIntegrationEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
