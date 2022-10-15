using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts;

public class ChangesProductCreateDto : IIntegrationEvent
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}
