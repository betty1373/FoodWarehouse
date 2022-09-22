using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientsGetAllEventHandler : IIntegrationEventHandler<IngredientsGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientsGetAllEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientsGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var ingredientsDto = await _ingredientsService.GetAll();

            await msgContext.RespondAsync(new IngredientsResponseDto 
            {
                Ingredients = ingredientsDto
            });
        }
    }
}