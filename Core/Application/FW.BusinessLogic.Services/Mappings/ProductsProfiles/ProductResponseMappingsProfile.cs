using AutoMapper;
using FW.BusinessLogic.Contracts.Products;
using FW.Domain;

namespace FW.BusinessLogic.Services.Mappings.ProductsProfiles;

public class ProductResponseMappingsProfile : Profile
{
    public ProductResponseMappingsProfile()
    {
        CreateMap<Products, ProductResponseDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Categories.Name))
            .ForMember(d => d.IngredientName, o => o.MapFrom(s => s.Ingredients.Name));
        CreateMap<ProductResponseDto, Products>()
            .ForMember(p => p.Warehouses, map => map.Ignore())
            .ForMember(p => p.Categories, map => map.Ignore())
            .ForMember(p => p.Ingredients, map => map.Ignore());
    }
}
