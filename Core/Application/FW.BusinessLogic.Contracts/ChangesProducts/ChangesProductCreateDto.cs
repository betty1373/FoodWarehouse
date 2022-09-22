using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts
{
    public class ChangesProductCreateDto : IIntegrationEvent
    {
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public Guid UserId { get; set; }
    }
}
