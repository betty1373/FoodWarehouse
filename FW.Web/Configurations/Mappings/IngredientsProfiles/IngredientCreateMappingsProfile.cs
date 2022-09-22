using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class IngredientCreateMappingsProfile : Profile
    {
        public IngredientCreateMappingsProfile()
        {         
            CreateMap<IngredientCreateDto, IngredientVM>();
            CreateMap<IngredientVM, IngredientCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore()); 
        }
    }
}
