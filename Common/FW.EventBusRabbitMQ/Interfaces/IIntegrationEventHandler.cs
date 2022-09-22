namespace FW.EventBus.Interfaces
{
    public interface IIntegrationEventHandler<TE> where TE : IIntegrationEvent
    {
        Task Handle(IntegrationContext<TE> @event);
    }
}
