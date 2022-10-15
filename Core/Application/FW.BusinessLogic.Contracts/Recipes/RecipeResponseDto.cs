namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipeResponseDto
    {
        public Guid Id { get; set; }

        public Guid IngredientId { get; set; }
    
        public Guid DishesId { get; set; }
        public int Quantity { get; set; }
    }
}
