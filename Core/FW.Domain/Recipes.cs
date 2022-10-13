using System.ComponentModel.DataAnnotations.Schema;

namespace FW.Domain;

public class Recipes : Base
{
    public Guid IngredientId { get; set; }
    public Guid DishesId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("IngredientId")]
    public Ingredients Ingredients { get; set; }
    [ForeignKey("DishesId")]
    public Dishes Dishes { get; set; }
}
