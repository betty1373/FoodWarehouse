using FW.EventBus.Interfaces;
using FW.EventBus;
using FW.Management.Services;

namespace FW.Management.Configurations
{
    public static class RabbitMQConfiguration
    {
        public static void ConfigureRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionRabbitMQ, ConnectionRabbitMQ>();
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();
        }
    }
}
