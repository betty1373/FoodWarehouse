using FW.EventBus.Interfaces;
using FW.Web.Configurations.Options;
using FW.Web.RpcClients.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace FW.Web.RpcClients
{
    public class RpcClientBase : IRpcClient
    {
        private IModel _channel;
        private readonly string _responseQueueNameEnding = "Response";
        private ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingMessages;
        private bool _disposed;

        public RpcClientBase(IConnectionRabbitMQ connection, IConfiguration configuration)
        {
            var _connection = connection.Get();
            _channel = _connection.CreateModel();

            _pendingMessages = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
        }

        private protected void ConfigureRpcClient(string exchangeName, string queueName)
        {
            ExchangeDeclare(exchangeName);
            QueueDeclare(queueName);
            var responseQueueName = queueName + _responseQueueNameEnding;
            QueueDeclare(responseQueueName);
            QueueBind(exchangeName, responseQueueName);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += MessageReceived_Handler;

            _channel.BasicConsume(responseQueueName, autoAck: false, consumer);
        }

        public async Task<string> CallAsync(string exchangeName, string queueName, string message)
        {
            var tcs = new TaskCompletionSource<string>();
            var correlationId = Guid.NewGuid().ToString();
            _pendingMessages[correlationId] = tcs;

            Publish(exchangeName, queueName, message, correlationId);

            return await tcs.Task;
        }

        private void Publish(string exchangeName, string queueName, string message, string correlationId)
        {
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = queueName + _responseQueueNameEnding;

            var messageBody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchangeName, queueName, properties, messageBody);
        }

        private async Task MessageReceived_Handler(object obj, BasicDeliverEventArgs args)
        {
            var correlationId = args.BasicProperties.CorrelationId;
            var response = Encoding.UTF8.GetString(args.Body.ToArray());

            _pendingMessages.TryRemove(correlationId, out var tcs);
            if (tcs != null)
                tcs.SetResult(response);

            _channel.BasicAck(args.DeliveryTag, false);
            await Task.CompletedTask;
        }

        #region Setting Exchange, Queue
        private void ExchangeDeclare(string exchangeName)
        {
            if (exchangeName.Length != 0)
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false);
        }
        private void QueueDeclare(string queueName)
        {
            _channel.QueueDeclare(queueName, true, false, autoDelete: false);
        }
        private void QueueBind(string exchangeName, string queueName)
        {
            if (exchangeName.Length != 0)
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
            if (!_disposed && disposing)
                this._channel?.Dispose();

            this._disposed = true;
        }
        #endregion
    }
}
