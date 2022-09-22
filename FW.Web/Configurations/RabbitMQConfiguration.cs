using FW.EventBus.Interfaces;
using FW.Web.RpcClients;
using FW.Web.RpcClients.Interfaces;
using FW.Web.Services;

namespace FW.Web.Configurations
{
    public static class RabbitMQConfiguration
    {
        public static void AddConnectionRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionRabbitMQ, ConnectionRabbitMQ>();
        }
    }
}
