using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services.Mappings.WarehousesProfiles
{
    public class WarehouseCreateMappingsProfile : Profile
    {
        public WarehouseCreateMappingsProfile()
        {
            CreateMap<Warehouses, WarehouseCreateDto>()
                .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
            CreateMap<WarehouseCreateDto, Warehouses>()
                .ForMember(p => p.Id, map => map.Ignore());
        }
    }
}