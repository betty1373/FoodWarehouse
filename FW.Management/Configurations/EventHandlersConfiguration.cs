using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Management.EventHandlers.ChangesProducts;
using FW.Management.EventHandlers.Dishes;
using FW.Management.EventHandlers.Ingredients;
using FW.Management.EventHandlers.Recipes;
using FW.Management.EventHandlers.Warehouses;
using FW.BusinessLogic.Contracts;
using FW.RabbitMQOptions;

namespace FW.Management.Configurations
{
    public static class EventHandlersConfiguration
    {
        public static void ConfigureEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<IIntegrationEventHandler<ChangesProductGetByIdDto>, ChangesProductGetByIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductsGetByParentIdDto>, ChangesProductsGetByParentIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductGetPageDto>, ChangesProductsGetPageEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductsGetAllDto>, ChangesProductsGetAllEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductsGetCountDto>, ChangesProductsGetCountEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductCreateDto>, ChangesProductCreateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductUpdateDto>, ChangesProductUpdateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<ChangesProductDeleteDto>, ChangesProductDeleteEventHandler>();

            services.AddScoped<IIntegrationEventHandler<DishGetByIdDto>, DishGetByIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishesGetPageDto>, DishesGetPageEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishesGetByParentIdDto>, DishesGetByParentIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishesGetAllDto>, DishesGetAllEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishesGetCountDto>, DishesGetCountEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishCreateDto>, DishCreateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishUpdateDto>, DishUpdateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishDeleteDto>, DishDeleteEventHandler>();
            services.AddScoped<IIntegrationEventHandler<DishCookDto>, DishCookEventHandler>();
          

            services.AddScoped<IIntegrationEventHandler<IngredientGetByIdDto>, IngredientGetByIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientsGetPageDto>, IngredientsGetPageEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientsGetAllDto>, IngredientsGetAllEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientsGetCountDto>, IngredientsGetCountEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientCreateDto>, IngredientCreateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientUpdateDto>, IngredientUpdateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<IngredientDeleteDto>, IngredientDeleteEventHandler>();

            services.AddScoped<IIntegrationEventHandler<RecipeGetByIdDto>, RecipeGetByIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipesGetByParentIdDto>, RecipesGetByParentIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipesGetPageDto>, RecipesGetPageEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipesGetAllDto>, RecipesGetAllEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipesGetCountDto>, RecipesGetCountEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipeCreateDto>, RecipeCreateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipeUpdateDto>, RecipeUpdateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RecipeDeleteDto>, RecipeDeleteEventHandler>();

            services.AddScoped<IIntegrationEventHandler<WarehouseGetByIdDto>, WarehouseGetByIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehouseGetByParentIdDto>, WarehouseGetByParentIdEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehousesGetPageDto>, WarehousesGetPageEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehousesGetAllDto>, WarehousesGetAllEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehousesGetCountDto>, WarehousesGetCountEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehouseCreateDto>, WarehouseCreateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehouseUpdateDto>, WarehouseUpdateEventHandler>();
            services.AddScoped<IIntegrationEventHandler<WarehouseDeleteDto>, WarehouseDeleteEventHandler>();
        }
    }
}
