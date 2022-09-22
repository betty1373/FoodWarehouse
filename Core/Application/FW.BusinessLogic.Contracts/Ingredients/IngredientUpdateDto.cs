using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Ingredients
{
    public class IngredientUpdateDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
