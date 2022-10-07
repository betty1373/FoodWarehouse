using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.RecipesProfiles;

public class RecipeUpdateMappingsProfile : Profile
{
    public RecipeUpdateMappingsProfile()
    {
        CreateMap<Recipes, RecipeUpdateDto>()
              .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
        CreateMap<RecipeUpdateDto, Recipes>()
            .ForMember(p => p.Ingredients, map => map.Ignore())
            .ForMember(p => p.Dishes, map => map.Ignore());
    }
}
