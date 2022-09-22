using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class CategoryInfoQueryMappingsProfile : Profile
    {
        public CategoryInfoQueryMappingsProfile()
        {
            CreateMap<CategoryResponseVM, CategoryResponseDto>();
            CreateMap<CategoryResponseDto, CategoryResponseVM>();
        }
    }
}
