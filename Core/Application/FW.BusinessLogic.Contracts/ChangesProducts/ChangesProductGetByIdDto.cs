using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts;

public class ChangesProductGetByIdDto : IIntegrationEvent
{
    public Guid Id { get; set; }
}
