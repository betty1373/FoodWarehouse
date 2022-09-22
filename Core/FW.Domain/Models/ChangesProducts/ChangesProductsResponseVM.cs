namespace FW.Domain.Models;
public class ChangesProductResponseVM : IEntity
    {
        public Guid Id { get; set; }
     //   public DateTime ModifiedOn { get; set; }
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
    }

