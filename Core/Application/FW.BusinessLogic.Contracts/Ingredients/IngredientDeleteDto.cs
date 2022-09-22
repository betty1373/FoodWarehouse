using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Ingredients
{
    public class IngredientDeleteDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
