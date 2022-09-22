using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipesGetByParentiIdDto : IIntegrationEvent
    {
        public Guid DishId { get; set; }
    }
}
