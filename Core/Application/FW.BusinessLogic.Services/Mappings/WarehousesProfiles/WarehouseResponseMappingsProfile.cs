using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.WarehousesProfiles
{
    public class WarehouseResponseMappingsProfile : Profile
    {
        public WarehouseResponseMappingsProfile()
        {
            CreateMap<Warehouses, WarehouseResponseDto>();
            // .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
            CreateMap<WarehouseResponseDto, Warehouses>();
             //   .ForMember(p => EF.Property<Guid?>(p, "UserId"),m => m.MapFrom(src=>src.UserId));
        }
    }
}