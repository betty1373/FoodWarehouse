using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts;

public class ChangesProductsGetByParentIdDto : IIntegrationEvent
{
    public Guid ProductId { get; set; }
}
