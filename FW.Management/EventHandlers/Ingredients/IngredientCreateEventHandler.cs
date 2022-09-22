using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientCreateEventHandler : IIntegrationEventHandler<IngredientCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientCreateEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientCreateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var ingredientId = await _ingredientsService.Create(msgContext.Message);

            if (ingredientId != Guid.Empty)
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = ingredientId,
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
