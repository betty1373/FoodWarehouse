using System.Text;
using System.Text.Json;
using FW.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FW.EventBus
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IServiceProvider _provider;
        private readonly IConnection _connection;
        private IModel _channel;
        private bool _disposed;

        public EventBusRabbitMQ(IServiceProvider provider, IConnectionRabbitMQ connection)
        {
            _provider = provider;
            _connection = connection.Get();
            _channel = _connection.CreateModel();
        }

        public void Publish(IIntegrationEvent @event, string queueName, string exchangeName = "")
        {
            ExchangeDeclare(exchangeName);
            QueueDeclare(queueName);

            var jsonMessage = JsonSerializer.Serialize(@event);
            var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(exchangeName, queueName, body: bytesMessage);
        }

        public void Subscribe<TH, TE>(string queueName = "",string exchangeName = "")
            where TH : IIntegrationEventHandler<TE>
            where TE : IIntegrationEvent
        {
            if(queueName == "")
                queueName = typeof(TE).Name;

            ExchangeDeclare(exchangeName);
            QueueDeclare(queueName);
            QueueBind(exchangeName, queueName);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (obj, args) =>
            {
                using (var scope = _provider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TE>>();

                    var correlationId = args.BasicProperties.CorrelationId;
                    var responseQueueName = args.BasicProperties.ReplyTo;
                    var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());
                    var message = JsonSerializer.Deserialize<TE>(jsonMessage);
                    string routingKey = args.RoutingKey;
                    var context = new IntegrationContext<TE>
                    {
                        CorrelationId = correlationId,
                        Message = message,
                        ExchangeName = exchangeName,
                        QueueName = queueName,
                        RespondAsync = async (msg) => 
                        {
                            if (msg == null)
                                throw new ArgumentNullException($"RespondAsync argument {nameof(msg)} in { typeof(IIntegrationEventHandler<TE>).Name} is null!");
                            
                            await RespondAsync(msg, correlationId, exchangeName, responseQueueName);
                        }
                    };
                    await handler.Handle(context);

                    _channel.BasicAck(args.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(string.Empty, false, consumer);
        }

        private Task RespondAsync(object responseMessage, string correlationId,string exchangeName, string responseQueueName)
        {
            var jsonMessage = JsonSerializer.Serialize(responseMessage);
            var responseMessageBytes = Encoding.UTF8.GetBytes(jsonMessage);

            var responseProps = _channel.CreateBasicProperties();
            responseProps.CorrelationId = correlationId;

            _channel.BasicPublish(exchangeName, responseQueueName, responseProps, responseMessageBytes);

            //_logger.Information($"Sent: {responseMessage} with CorrelationId {correlationId}");
            return Task.CompletedTask;
        }

        #region Setting Exchange, Queue
        private void ExchangeDeclare(string exchangeName)
        {
            if (exchangeName != "")
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false);
        }
        private void QueueDeclare(string queueName)
        {
            _channel.QueueDeclare(queueName, true, false, autoDelete: false);
        }
        private void QueueBind(string exchangeName, string queueName)
        {
            if (exchangeName != "")
                _channel.QueueBind(queueName, exchangeName, queueName);
        }
        #endregion

        #region Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _channel?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
