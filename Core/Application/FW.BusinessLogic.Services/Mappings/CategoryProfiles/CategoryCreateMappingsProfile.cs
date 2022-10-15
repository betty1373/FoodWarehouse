using AutoMapper;
using FW.BusinessLogic.Contracts.Category;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.CategoryProfiles;

public class CategoryCreateMappingsProfile : Profile
{
    public CategoryCreateMappingsProfile()
    {
        CreateMap<Categories, CategoryCreateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
        CreateMap<CategoryCreateDto, Categories>()
            .ForMember(p => p.Id, map => map.Ignore());
    }
}