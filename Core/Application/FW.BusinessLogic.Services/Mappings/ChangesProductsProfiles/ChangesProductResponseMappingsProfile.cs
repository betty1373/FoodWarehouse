using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles;

public class ChangesProductResponseMappingsProfile : Profile
{
    public ChangesProductResponseMappingsProfile()
    {       
        CreateMap<ChangesProductResponseDto, ChangesProducts>()
            .ForMember(p => p.Products, map => map.Ignore());
        CreateMap<ChangesProducts, ChangesProductResponseDto>();
    }
}