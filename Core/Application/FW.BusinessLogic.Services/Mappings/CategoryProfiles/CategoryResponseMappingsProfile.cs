using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain;

namespace FW.BusinessLogic.Services.Mappings.CategoryProfiles
{
    public class CategoryResponseMappingsProfile : Profile
    {
        public CategoryResponseMappingsProfile()
        {
            CreateMap<Categories, CategoryResponseDto>();
            CreateMap<CategoryResponseDto, Categories>();
        }
    }
}