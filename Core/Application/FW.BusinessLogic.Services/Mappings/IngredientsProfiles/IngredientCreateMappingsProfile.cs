using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.IngredientsProfiles;

public class IngredientCreateMappingsProfile : Profile
{
    public IngredientCreateMappingsProfile()
    {
        CreateMap<Ingredients, IngredientCreateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); 
        CreateMap<IngredientCreateDto, Ingredients>()
            .ForMember(p => p.Id, map => map.Ignore())
            ;
    }
}