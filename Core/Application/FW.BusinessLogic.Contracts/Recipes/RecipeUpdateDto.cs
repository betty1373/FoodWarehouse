using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipeUpdateDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid IngredientId { get; set; }
        public Guid DishesId { get; set; }
        public double Quantity { get; set; }
        public Guid UserId { get; set; }
    }
}
