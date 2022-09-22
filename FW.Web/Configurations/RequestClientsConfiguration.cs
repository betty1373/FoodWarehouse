using FW.Web.RequestClients;
using FW.Web.RequestClients.Interfaces;

namespace FW.Web.Configurations
{
    public static class RequestClientsConfiguration
    {
        public static void AddRequestClients(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRequestClient, CategoriesRequestClient>();
            services.AddScoped<IProductsRequestClient, ProductsRequestClient>();
        }
    }
}
