using AutoMapper;
using FW.BusinessLogic.Contracts.Products;
using FW.Domain;
using Microsoft.EntityFrameworkCore;
namespace FW.BusinessLogic.Services.Mappings.ProductsProfiles;
/// <summary>
/// Профиль автомаппера для сущности Продукты.
/// </summary>
public class ProductCreateMappingsProfile : Profile
{
    public ProductCreateMappingsProfile()
    {
        CreateMap<Products, ProductCreateDto>()
            .ForMember(p => p.UserId, m => m.MapFrom(src => EF.Property<Guid?>(src, "UserId"))); ;
        CreateMap<ProductCreateDto, Products>()
            .ForMember(p => p.Id, map => map.Ignore())
            .ForMember(p => p.Warehouses, map => map.Ignore())
            .ForMember(p => p.Categories, map => map.Ignore())
            .ForMember(p => p.Ingredients, map => map.Ignore());
    }
}
