using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Recipes
{
    public class RecipesGetPageDto : IIntegrationEvent
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
