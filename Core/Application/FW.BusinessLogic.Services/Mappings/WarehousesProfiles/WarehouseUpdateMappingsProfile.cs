using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.WarehousesProfiles;

public class WarehouseUpdateMappingsProfile : Profile
{
    public WarehouseUpdateMappingsProfile()
    {
        CreateMap<Warehouses, WarehouseUpdateDto>()
          .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
        CreateMap<WarehouseUpdateDto, Warehouses>();
    }
}
