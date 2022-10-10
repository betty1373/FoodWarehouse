using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Domain;
namespace FW.BusinessLogic.Services.Mappings.RecipesProfiles;

public class RecipeResponseMappingsProfile : Profile
{
    public RecipeResponseMappingsProfile()
    {
        CreateMap<Recipes, RecipeResponseDto>();
           // .ForMember(d => d.IngredientName, o => o.MapFrom(s => s.Ingredients.Name));
//.AfterMap((s, d, context) => context.Mapper.Map(s.Concert, d));
        //      .ForMember(p => p.IngredientName, map => map.Ignore());
        CreateMap<RecipeResponseDto, Recipes>()
            .ForMember(p => p.Ingredients, map => map.Ignore())
            .ForMember(p => p.Dishes, map => map.Ignore());
    }
}