using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    public class ChangesProductInfoQueryMappingsProfile : Profile
    {
        public ChangesProductInfoQueryMappingsProfile()
        {
            CreateMap<ChangesProductResponseVM, ChangesProductResponseDto>();
            CreateMap<ChangesProductResponseDto, ChangesProductResponseVM>();
        }
    }
}
