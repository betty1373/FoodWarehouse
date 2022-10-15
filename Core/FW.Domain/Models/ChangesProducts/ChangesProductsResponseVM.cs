namespace FW.Domain.Models;
public class ChangesProductResponseVM : IEntity
{
    public Guid Id { get; set; }   
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime ModifiedOn { get; set; }
}

