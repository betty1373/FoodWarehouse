using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.RecipesProfiles
{
    public class RecipeCreateMappingsProfile : Profile
    {
        public RecipeCreateMappingsProfile()
        {
            CreateMap<Recipes, RecipeCreateDto>()
                .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
            CreateMap<RecipeCreateDto, Recipes>()
                .ForMember(p => p.Id, map => map.Ignore())
                .ForMember(p => p.Ingredients, map => map.Ignore())
                .ForMember(p => p.Dishes, map => map.Ignore());
        }
    }
}