using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipeCreateEventHandler : IIntegrationEventHandler<RecipeCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipeCreateEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipeCreateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var recipeId = await _recipesService.Create(msgContext.Message);

            if (recipeId != Guid.Empty)
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = recipeId,
                    Status = StatusResult.Ok,
                    Title = "Added"
                });
            else
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.Error,
                    Title = "Error"
                });
        }
    }
}
