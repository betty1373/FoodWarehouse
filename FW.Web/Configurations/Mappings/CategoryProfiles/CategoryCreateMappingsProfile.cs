using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class CategoryCreateMappingsProfile : Profile
    {
        public CategoryCreateMappingsProfile()
        {
            CreateMap<CategoryCreateDto, CategoryVM>();
            CreateMap<CategoryVM, CategoryCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
