using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientGetByIdEventHandler : IIntegrationEventHandler<IngredientGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientGetByIdEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var ingredientId = msgContext.Message.Id;
            var ingredientDto = await _ingredientsService.GetById(ingredientId);

            if (ingredientDto != null)
                await msgContext.RespondAsync(ingredientDto);
            else
                await msgContext.RespondAsync(new IngredientResponseDto{});
        }
    }
}
