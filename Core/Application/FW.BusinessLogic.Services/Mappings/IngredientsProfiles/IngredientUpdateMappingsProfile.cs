using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.IngredientsProfiles
{
    public class IngredientUpdateMappingsProfile : Profile
    {
        public IngredientUpdateMappingsProfile()
        {
            CreateMap<Ingredients, IngredientUpdateDto>()
                .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
            CreateMap<IngredientUpdateDto, Ingredients>();
              
        }
    }
}
