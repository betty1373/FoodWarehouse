using AutoMapper;
using FW.BusinessLogic.Contracts.Warehouses;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    public class WarehouseUpdateMappingsProfile : Profile
    {
        public WarehouseUpdateMappingsProfile()
        {
            CreateMap<WarehouseUpdateDto, WarehouseVM>();
            CreateMap<WarehouseVM, WarehouseUpdateDto>()
                .ForMember(p => p.Id, map => map.Ignore())
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
