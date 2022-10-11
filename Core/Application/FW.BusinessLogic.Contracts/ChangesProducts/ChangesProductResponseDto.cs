namespace FW.BusinessLogic.Contracts.ChangesProducts;

public class ChangesProductResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public double Quantity { get; set; }
    public DateTime ModifiedOn { get; set; }
}
