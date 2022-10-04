using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipesGetByParentIdDto : IIntegrationEvent
    {
        public Guid DishId { get; set; }
    }
}
