using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientsGetPageEventHandler : IIntegrationEventHandler<IngredientsGetPageDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientsGetPageEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientsGetPageDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            int page = msgContext.Message.Skip;
            int pageSize = msgContext.Message.Take;
            var ingredientsDto = await _ingredientsService.GetPaged(page, pageSize);

            await msgContext.RespondAsync(new IngredientsResponseDto 
            {
                Ingredients = ingredientsDto
            });
        }
    }
}