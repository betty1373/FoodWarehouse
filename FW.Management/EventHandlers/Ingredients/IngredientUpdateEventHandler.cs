using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientUpdateEventHandler : IIntegrationEventHandler<IngredientUpdateDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientUpdateEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientUpdateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var ingredientDto = msgContext.Message;
            var status = await _ingredientsService.Update(ingredientDto);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = ingredientDto.Id,
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
