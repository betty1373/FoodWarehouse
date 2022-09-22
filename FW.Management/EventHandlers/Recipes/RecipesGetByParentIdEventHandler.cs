using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Recipes
{
    public class RecipesGetByParentIdEventHandler : IIntegrationEventHandler<RecipesGetByParentiIdDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public RecipesGetByParentIdEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<RecipesGetByParentiIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishId = msgContext.Message.DishId;
            var recipesDto = await _recipesService.GetByParentId(dishId);

            await msgContext.RespondAsync(new RecipesResponseDto { Recipes = recipesDto });
        }
    }
}
