using AutoMapper;
using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.Models;
namespace FW.Web.Configurations.Mappings
{
    public class ChangesProductCreateMappingsProfile : Profile
    {
        public ChangesProductCreateMappingsProfile()
        {
            CreateMap<ChangesProductCreateDto, ChangesProductVM>();
            CreateMap<ChangesProductVM, ChangesProductCreateDto>()
                .ForMember(p => p.UserId, map => map.Ignore()); 
        }
    }
}
