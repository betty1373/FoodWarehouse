namespace FW.Models;
public class ProductVM
{
    public Guid WarehouseId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid IngredientId { get; set; }

    public DateTime ExpirationDate { get; set; }
    public int Quantity { get; set; }
}

