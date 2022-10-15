using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    public class RecipeInfoQueryMappingsProfile : Profile
    {
        public RecipeInfoQueryMappingsProfile()
        {
            CreateMap<RecipeResponseVM, RecipeResponseDto>();
            CreateMap<RecipeResponseDto, RecipeResponseVM>();
        }
    }
}
