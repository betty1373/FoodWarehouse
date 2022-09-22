using RabbitMQ.Client;

namespace FW.EventBus.Interfaces
{
    public interface IConnectionRabbitMQ : IDisposable
    {
        public IConnection Get();
    }
}
