using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    public class WarehouseCreateMappingsProfile : Profile
    {
        public WarehouseCreateMappingsProfile()
        {
            CreateMap<WarehouseCreateDto, WarehouseVM>();
            CreateMap<WarehouseVM, WarehouseCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore()); 
        }
    }
}
