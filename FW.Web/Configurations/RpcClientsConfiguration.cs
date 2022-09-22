using FW.Web.RpcClients;
using FW.Web.RpcClients.Interfaces;

namespace FW.Web.Configurations
{
    public static class RpcClientsConfiguration
    {
        public static void AddRpcClients(this IServiceCollection services)
        {
            services.AddSingleton<IChangesProductsRpcClient, ChangesProductsRpcClient>();
            services.AddSingleton<IDishesRpcClient, DishesRpcClient>();
            services.AddSingleton<IIngredientsRpcClient, IngredientsRpcClient>();
            services.AddSingleton<IRecipesRpcClient, RecipesRpcClient>();
            services.AddSingleton<IWarehousesRpcClient, WarehousesRpcClient>();
        }
    }
}
