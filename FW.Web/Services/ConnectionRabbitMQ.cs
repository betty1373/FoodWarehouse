using FW.EventBus.Interfaces;
using FW.RabbitMQOptions;
using RabbitMQ.Client;
using Serilog;

namespace FW.Web.Services
{
    public class ConnectionRabbitMQ : IConnectionRabbitMQ
    {
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private bool _disposed;

        public ConnectionRabbitMQ(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;

            var rabbitMqOptions = configuration.GetSection(RabbitMqConnectionOptions.KeyValue).Get<RabbitMqConnectionOptions>();

            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    ClientProvidedName = rabbitMqOptions.ClientName,
                    HostName = rabbitMqOptions.HostName,
                    VirtualHost = rabbitMqOptions.VirtualHost,
                    UserName = rabbitMqOptions.UserName,
                    Password = rabbitMqOptions.Password,
                    DispatchConsumersAsync = true
                };

                _connection = connectionFactory.CreateConnection();
                _logger.Debug($"Create {typeof(ConnectionFactory)} - {rabbitMqOptions.HostName}");
            }
            catch
            {
                _logger.Debug($"Error {typeof(ConnectionFactory)} - {rabbitMqOptions.HostName}");
            }
        }

        public IConnection Get()
        {
            return _connection;
        }

        #region Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _connection?.Dispose();

            _disposed = true;
        }
        #endregion
    }
}
