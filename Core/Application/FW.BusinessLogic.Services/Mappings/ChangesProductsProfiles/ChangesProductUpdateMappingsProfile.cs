using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles;

public class ChangesProductUpdateMappingsProfile : Profile
{
    public ChangesProductUpdateMappingsProfile()
    {
        CreateMap<ChangesProducts, ChangesProductUpdateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
        CreateMap<ChangesProductUpdateDto, ChangesProducts>()
            .ForMember(p => p.Products, map => map.Ignore());
    }
}
