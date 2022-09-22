using AutoMapper;
using FW.BusinessLogic.Contracts.Dishes;
using FW.Domain;

namespace FW.BusinessLogic.Services.Mappings.DishesProfiles
{
    public class DishResponseMappingsProfile : Profile
    {
        public DishResponseMappingsProfile()
        {
            CreateMap<Dishes, DishResponseDto>();
            CreateMap<DishResponseDto, Dishes>();
        }
    }
}