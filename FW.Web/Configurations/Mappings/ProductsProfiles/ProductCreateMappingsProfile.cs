using AutoMapper;
using FW.BusinessLogic.Contracts.Products;
using FW.Models;

namespace FW.Web.Configurations.Mappings
{
    /// <summary>
    /// Профиль автомаппера для сущности Продукты.
    /// </summary>
    public class ProductCreateMappingsProfile:Profile
    {
        public ProductCreateMappingsProfile()
        {
            CreateMap<ProductCreateDto, ProductVM>();
            CreateMap<ProductVM, ProductCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore());
        }
    }
}
