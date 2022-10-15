using FW.EventBus.Interfaces;

namespace FW.BusinessLogic.Contracts.Ingredients;

public class IngredientsGetPageDto : IIntegrationEvent
{
    public int Skip { get; set; }
    public int Take { get; set; }
}
