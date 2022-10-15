namespace FW.Domain.Models; 
public class ProductResponseVM : IEntity
    {
        public Guid Id { get; set; }
     
        public Guid WarehouseId { get; set; }
        public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; }
   
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
    }

