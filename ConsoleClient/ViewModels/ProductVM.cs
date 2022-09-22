namespace ConsoleClient.ViewModels
{
    public class ProductVM
    {
        public Guid WarehouseId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Quantity { get; set; }
    }
}
