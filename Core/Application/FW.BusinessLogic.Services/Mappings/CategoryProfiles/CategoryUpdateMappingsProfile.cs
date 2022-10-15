using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.CategoryProfiles;
public class CategoryUpdateMappingsProfile : Profile
{
    public CategoryUpdateMappingsProfile()
    {
        CreateMap<Categories, CategoryUpdateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
        CreateMap<CategoryUpdateDto, Categories>()
          
            ;
    }
}
