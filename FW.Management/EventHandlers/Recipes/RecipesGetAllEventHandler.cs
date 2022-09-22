using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipesGetAllEventHandler : IIntegrationEventHandler<RecipesGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipesGetAllEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipesGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var recipesDto = await _recipesService.GetAll();

            await msgContext.RespondAsync(new RecipesResponseDto 
            {
                Recipes = recipesDto
            });
        }
    }
}
