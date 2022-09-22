using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain.Models;
namespace FW.Web.Configurations.Mappings
{
    public class CategoryUpdateMappingsProfile : Profile
    {
        public CategoryUpdateMappingsProfile()
        {
            CreateMap<CategoryUpdateDto, CategoryVM>();
            CreateMap<CategoryVM, CategoryUpdateDto>()
                .ForMember(p => p.Id, map => map.Ignore())
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
