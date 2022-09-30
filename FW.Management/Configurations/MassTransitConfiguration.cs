using FW.Management.Consumers.CategoryConsumer;
using FW.Management.Consumers.ProductsConsumers;
using FW.RabbitMQOptions;
using MassTransit;

namespace FW.Management.Configurations
{
    public static class MassTransitConfiguration
    {
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = configuration.GetSection(RabbitMqConnectionOptions.KeyValue).Get<RabbitMqConnectionOptions>();

            services.AddMassTransit(x =>
            {
                x.SetSnakeCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host($"rabbitmq://{rabbitMqOptions.HostName}/{rabbitMqOptions.VirtualHost}", hostCfg =>
                    {
                        hostCfg.Username(rabbitMqOptions.UserName);
                        hostCfg.Password(rabbitMqOptions.Password);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance);
                });

                x.AddConsumer<ProductCreateConsumer>();
                x.AddConsumer<ProductGetByIdConsumer>();
                x.AddConsumer<ProductsGetByParentIdConsumer>();
                x.AddConsumer<ProductUpdateConsumer>();
                x.AddConsumer<ProductDeleteConsumer>();
                x.AddConsumer<ProductsGetPageConsumer>();
                x.AddConsumer<ProductsGetAllConsumer>();
                x.AddConsumer<ProductsGetCountConsumer>();

                x.AddConsumer<CategoryCreateConsumer>();
                x.AddConsumer<CategoryGetByIdConsumer>();
                x.AddConsumer<CategoryUpdateConsumer>();
                x.AddConsumer<CategoryDeleteConsumer>();
                x.AddConsumer<CategoriesGetPageConsumer>();
                x.AddConsumer<CategoriesGetAllConsumer>();
                x.AddConsumer<CategoriesGetCountConsumer>();
            });

          //  services.AddMassTransitHostedService();
        }
    }
}
