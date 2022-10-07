using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FW.Domain;

public class Products : Base
{
    public Guid WarehouseId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid IngredientId { get; set; }
    //[Required]
    //public string Name { get; set; }
    public DateTime ExpirationDate { get; set; }
    public double Quantity { get; set; }

    [ForeignKey("WarehouseId")]
    public Warehouses Warehouses { get; set; }
    [ForeignKey("CategoryId")]
    public Categories Categories { get; set; }
    [ForeignKey("IngredientId")]
    public Ingredients Ingredients { get; set; }
}
