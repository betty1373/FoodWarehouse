using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.DishesProfiles;

public class DishCreateMappingsProfile : Profile
{
    public DishCreateMappingsProfile()
    {
        CreateMap<Dishes, DishCreateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
        CreateMap<DishCreateDto, Dishes>()
            .ForMember(p => p.Id, map => map.Ignore())
            //.ForMember(p => p.ModifiedOn, map => map.Ignore())
            ;
    }
}