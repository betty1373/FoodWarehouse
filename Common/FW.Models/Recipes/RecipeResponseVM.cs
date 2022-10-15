
namespace FW.Models;
public class RecipeResponseVM : IEntity
{
    public Guid Id { get; set; }

    public Guid IngredientId { get; set; }

    public Guid DishesId { get; set; }
    public int Quantity { get; set; }
}

