using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class ChangesProductUpdateMappingsProfile : Profile
    {
        public ChangesProductUpdateMappingsProfile()
        {
            CreateMap<ChangesProductUpdateDto, ChangesProductVM>();
            CreateMap<ChangesProductVM, ChangesProductUpdateDto>()
                .ForMember(p => p.Id, map => map.Ignore())
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
