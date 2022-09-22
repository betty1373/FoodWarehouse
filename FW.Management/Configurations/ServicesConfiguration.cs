using FW.BusinessLogic.Services;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();
            
            services.AddScoped<IChangesProductsService, ChangesProductsService>();
            services.AddScoped<IDishesService, DishesService>();
            services.AddScoped<IIngredientsService, IngredientsService>();
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<IWarehousesService, WarehousesService>();
        }
    }
}
