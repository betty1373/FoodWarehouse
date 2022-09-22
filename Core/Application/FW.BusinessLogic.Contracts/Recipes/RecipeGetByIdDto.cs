using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipeGetByIdDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
