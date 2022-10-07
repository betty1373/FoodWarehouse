using AutoMapper;
using FW.BusinessLogic.Contracts.Products;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.ProductsProfiles;

public class ProductUpdateMappingsProfile : Profile
{
    public ProductUpdateMappingsProfile()
    {
        CreateMap<Products, ProductUpdateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId")));
        CreateMap<ProductUpdateDto, Products>()
            .ForMember(p => p.Warehouses, map => map.Ignore())
            .ForMember(p => p.Categories, map => map.Ignore())
            .ForMember(p => p.Ingredients, map => map.Ignore());
    }
}
