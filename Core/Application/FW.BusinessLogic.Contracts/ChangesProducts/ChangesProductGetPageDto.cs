using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.ChangesProducts
{
    public class ChangesProductGetPageDto : IIntegrationEvent
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
