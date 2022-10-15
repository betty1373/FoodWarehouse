using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts;

public class ChangesProductUpdateDto : IIntegrationEvent
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}
