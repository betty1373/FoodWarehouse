using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Models;
namespace FW.Web.Configurations.Mappings
{
    public class WarehouseInfoQueryMappingsProfile : Profile
    {
        public WarehouseInfoQueryMappingsProfile()
        {
            CreateMap<WarehouseResponseVM, WarehouseResponseDto>();
            CreateMap<WarehouseResponseDto, WarehouseResponseVM>();
        }
    }
}
