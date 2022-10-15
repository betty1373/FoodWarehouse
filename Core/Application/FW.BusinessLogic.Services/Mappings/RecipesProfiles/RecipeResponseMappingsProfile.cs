using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Domain;
namespace FW.BusinessLogic.Services.Mappings.RecipesProfiles;

public class RecipeResponseMappingsProfile : Profile
{
    public RecipeResponseMappingsProfile()
    {
        CreateMap<Recipes, RecipeResponseDto>();
        CreateMap<RecipeResponseDto, Recipes>()
            .ForMember(p => p.Ingredients, map => map.Ignore())
            .ForMember(p => p.Dishes, map => map.Ignore());
    }
}