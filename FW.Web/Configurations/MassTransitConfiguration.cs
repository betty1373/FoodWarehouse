using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Contracts.Products;
using FW.Web.Configurations.Options;
using MassTransit;
using FW.RabbitMQOptions;

namespace FW.Web.Configurations
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
                    cfg.Host($"rabbitmq://{rabbitMqOptions.HostName }/{rabbitMqOptions.VirtualHost}", hostCfg =>
                    {
                        hostCfg.Username(rabbitMqOptions.UserName);
                        hostCfg.Password(rabbitMqOptions.Password);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance);
                });

                x.AddRequestClient<ProductGetByIdDto>();
                x.AddRequestClient<ProductsGetPageDto>();
                x.AddRequestClient<ProductsGetAllDto>();
                x.AddRequestClient<ProductsGetCountDto>();
                x.AddRequestClient<ProductCreateDto>();
                x.AddRequestClient<ProductUpdateDto>();
                x.AddRequestClient<ProductDeleteDto>();


                x.AddRequestClient<CategoryGetByIdDto>();
                x.AddRequestClient<CategoriesGetPageDto>();
                x.AddRequestClient<CategoriesGetAllDto>();
                x.AddRequestClient<CategoriesGetCountDto>();
                x.AddRequestClient<CategoryCreateDto>();
                x.AddRequestClient<CategoryUpdateDto>();
                x.AddRequestClient<CategoryDeleteDto>();
            });

        }
    }
}
