using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.ChangesProductsProfiles;
public class ChangesProductCreateMappingsProfile : Profile
{
    public ChangesProductCreateMappingsProfile()
    {
        CreateMap<ChangesProducts, ChangesProductCreateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
        CreateMap<ChangesProductCreateDto, ChangesProducts>()
            .ForMember(p => p.Id, map => map.Ignore())
         //   .ForMember(p => p.ModifiedOn, map => map.Ignore())
            .ForMember(p => p.Products, map => map.Ignore());
    }
}