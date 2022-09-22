using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipeUpdateEventHandler : IIntegrationEventHandler<RecipeUpdateDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipeUpdateEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipeUpdateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var recipeDto = msgContext.Message;
            var status = await _recipesService.Update(recipeDto);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = recipeDto.Id,
                    Status = StatusResult.Ok,
                    Title = "Updated"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
