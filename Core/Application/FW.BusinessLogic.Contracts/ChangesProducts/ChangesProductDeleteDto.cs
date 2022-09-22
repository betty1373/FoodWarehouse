using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts
{
    public class ChangesProductDeleteDto : IIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
