using AutoMapper;
using FW.BusinessLogic.Contracts.Products;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class ProductInfoQueryMappingsProfile : Profile
    {
        public ProductInfoQueryMappingsProfile()
        {
            CreateMap<ProductResponseVM, ProductResponseDto>();
            CreateMap<ProductResponseDto, ProductResponseVM>();
        }
    }
}
