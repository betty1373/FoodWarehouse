using AutoMapper;
using FW.BusinessLogic.Contracts.Recipes;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    public class RecipeUpdateMappingsProfile : Profile
    {
        public RecipeUpdateMappingsProfile()
        {
            CreateMap<RecipeUpdateDto, RecipeVM>();
            CreateMap<RecipeVM, RecipeUpdateDto>()
                .ForMember(p => p.Id, map => map.Ignore())
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
