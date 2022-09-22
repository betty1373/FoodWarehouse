using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Ingredients
{
    public class IngredientGetByIdDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
