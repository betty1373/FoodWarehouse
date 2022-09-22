using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain;

namespace FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles
{
    public class ChangesProductResponseMappingsProfile : Profile
    {
        public ChangesProductResponseMappingsProfile()
        {
            CreateMap<ChangesProducts, ChangesProductResponseDto>();
            CreateMap<ChangesProductResponseDto, ChangesProducts>()
                .ForMember(p => p.Products, map => map.Ignore());
        }
    }
}