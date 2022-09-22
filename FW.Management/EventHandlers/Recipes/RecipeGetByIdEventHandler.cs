using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipeGetByIdEventHandler : IIntegrationEventHandler<RecipeGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipeGetByIdEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipeGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var recipeId = msgContext.Message.Id;
            var recipeDto = await _recipesService.GetById(recipeId);

            if (recipeDto != null)
                await msgContext.RespondAsync(recipeDto);
            else
                await msgContext.RespondAsync(new RecipeResponseDto {});
        }
    }
}
