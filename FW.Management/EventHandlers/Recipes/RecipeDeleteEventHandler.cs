using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipeDeleteEventHandler : IIntegrationEventHandler<RecipeDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipeDeleteEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipeDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var recipeId = msgContext.Message.Id;
            var status = await _recipesService.Delete(recipeId);
            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = recipeId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = recipeId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
