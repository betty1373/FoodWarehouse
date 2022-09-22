using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class IngredientInfoQueryMappingsProfile : Profile
    {
        public IngredientInfoQueryMappingsProfile()
        {
            CreateMap<IngredientResponseVM, IngredientResponseDto>();
            CreateMap<IngredientResponseDto, IngredientResponseVM>();
        }
    }
}
