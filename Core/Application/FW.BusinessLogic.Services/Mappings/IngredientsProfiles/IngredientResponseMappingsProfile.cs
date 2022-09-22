using AutoMapper;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.Domain;

namespace FW.BusinessLogic.Services.Mappings.IngredientsProfiles
{
    public class IngredientResponseMappingsProfile : Profile
    {
        public IngredientResponseMappingsProfile()
        {
            CreateMap<Ingredients, IngredientResponseDto>();
            CreateMap<IngredientResponseDto, Ingredients>();
        }
    }
}