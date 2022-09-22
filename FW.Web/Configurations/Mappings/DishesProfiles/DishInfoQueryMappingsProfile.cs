using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class DishInfoQueryMappingsProfile : Profile
    {
        public DishInfoQueryMappingsProfile()
        {
            CreateMap<DishResponseVM, DishResponseDto>();
            CreateMap<DishResponseDto, DishResponseVM>();
        }
    }
}
