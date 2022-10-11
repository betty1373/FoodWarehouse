using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain;
using Microsoft.EntityFrameworkCore;

namespace FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles;

public class ChangesProductResponseMappingsProfile : Profile
{
    public ChangesProductResponseMappingsProfile()
    {
        CreateMap<ChangesProducts, ChangesProductResponseDto>()
            .ForMember(p => p.ModifiedOn, m => m.MapFrom(src => EF.Property<DateTime?>(src, "ModifiedOn"))); ;
        CreateMap<ChangesProductResponseDto, ChangesProducts>()
            .ForMember(p => p.Products, map => map.Ignore())
            .ForMember(p => p.Products, map => map.Ignore());
    }
}