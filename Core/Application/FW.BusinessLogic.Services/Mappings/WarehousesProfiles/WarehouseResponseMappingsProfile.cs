using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Domain;
namespace FW.BusinessLogic.Services.Mappings.WarehousesProfiles;
public class WarehouseResponseMappingsProfile : Profile
{
    public WarehouseResponseMappingsProfile()
    {
        CreateMap<Warehouses, WarehouseResponseDto>();
        CreateMap<WarehouseResponseDto, Warehouses>();
    }
}