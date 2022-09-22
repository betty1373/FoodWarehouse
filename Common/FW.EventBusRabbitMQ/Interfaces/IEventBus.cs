namespace FW.EventBus.Interfaces
{
    public interface IEventBus : IDisposable
    {
        void Subscribe<TH, TE>(string queueName = "", string exchangeName = "")
            where TH : IIntegrationEventHandler<TE>
            where TE : IIntegrationEvent;

        void Publish(IIntegrationEvent @event, string queueName, string exchangeName = "");
    }
}