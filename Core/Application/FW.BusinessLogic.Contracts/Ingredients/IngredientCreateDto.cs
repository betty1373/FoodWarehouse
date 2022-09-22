using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Ingredients
{
    public class IngredientCreateDto : IIntegrationEvent
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
