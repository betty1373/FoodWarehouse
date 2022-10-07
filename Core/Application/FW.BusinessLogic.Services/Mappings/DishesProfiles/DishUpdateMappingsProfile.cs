using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.DishesProfiles;

public class DishUpdateMappingsProfile : Profile
{
    public DishUpdateMappingsProfile()
    {
        CreateMap<Dishes, DishUpdateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
        CreateMap<DishUpdateDto, Dishes>()
            //.ForMember(p => p.ModifiedOn, map => map.Ignore())
            ;
    }
}
