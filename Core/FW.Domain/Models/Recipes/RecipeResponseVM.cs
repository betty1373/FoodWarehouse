namespace FW.Domain.Models;
public class RecipeResponseVM : IEntity
    {
        public Guid Id { get; set; }
     //   public DateTime ModifiedOn { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public Guid DishesId { get; set; }
        public double Quantity { get; set; }
    }

