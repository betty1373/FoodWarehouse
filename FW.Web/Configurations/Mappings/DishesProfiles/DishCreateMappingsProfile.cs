using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.Domain.Models;

namespace FW.Web.Configurations.Mappings
{
    public class DishCreateMappingsProfile : Profile
    {
        public DishCreateMappingsProfile()
        {
            CreateMap<DishCreateDto, DishVM>();
            CreateMap<DishVM, DishCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore()); 
        }
    }
}
