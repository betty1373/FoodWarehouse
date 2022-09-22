using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts.Recipes;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipesGetCountEventHandler : IIntegrationEventHandler<RecipesGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipesGetCountEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipesGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var count = await _recipesService.Count();

            await msgContext.RespondAsync(count);
        }
    }
}
