using AutoMapper;
using FW.BusinessLogic.Services.Mappings.CategoryProfiles;
using FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles;
using FW.BusinessLogic.Services.Mappings.DishesProfiles;
using FW.BusinessLogic.Services.Mappings.IngredientsProfiles;
using FW.BusinessLogic.Services.Mappings.ProductsProfiles;
using FW.BusinessLogic.Services.Mappings.RecipesProfiles;
using FW.BusinessLogic.Services.Mappings.WarehousesProfiles;

namespace FW.Management.Configurations;

public static class AutoMapperConfiguration
{
    public static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(new Mapper(Configuration()));
    }
    private static MapperConfiguration Configuration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryCreateMappingsProfile>();
            cfg.AddProfile<CategoryUpdateMappingsProfile>();
            cfg.AddProfile<CategoryResponseMappingsProfile>();

            cfg.AddProfile<ChangesProductCreateMappingsProfile>();
            cfg.AddProfile<ChangesProductUpdateMappingsProfile>();
            cfg.AddProfile<ChangesProductResponseMappingsProfile>();

            cfg.AddProfile<DishCreateMappingsProfile>();
            cfg.AddProfile<DishUpdateMappingsProfile>();
            cfg.AddProfile<DishResponseMappingsProfile>();

            cfg.AddProfile<IngredientCreateMappingsProfile>();
            cfg.AddProfile<IngredientUpdateMappingsProfile>();
            cfg.AddProfile<IngredientResponseMappingsProfile>();

            cfg.AddProfile<ProductCreateMappingsProfile>();
            cfg.AddProfile<ProductUpdateMappingsProfile>();
            cfg.AddProfile<ProductResponseMappingsProfile>();

            cfg.AddProfile<RecipeCreateMappingsProfile>();
            cfg.AddProfile<RecipeUpdateMappingsProfile>();
            cfg.AddProfile<RecipeResponseMappingsProfile>();


            cfg.AddProfile<WarehouseCreateMappingsProfile>();
            cfg.AddProfile<WarehouseUpdateMappingsProfile>();
            cfg.AddProfile<WarehouseResponseMappingsProfile>();
        });

        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}
