using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class RecipeCreateMappingsProfile : Profile
    {
        public RecipeCreateMappingsProfile()
        {
            CreateMap<RecipeCreateDto, RecipeVM>();
            CreateMap<RecipeVM, RecipeCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore()); ;
        }
    }
}
