using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts.Ingredients;

namespace FW.Management.EventHandlers.Ingredients
{
    public class IngredientsGetCountEventHandler : IIntegrationEventHandler<IngredientsGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IIngredientsService _ingredientsService;

        public IngredientsGetCountEventHandler(ILogger logger, IIngredientsService ingredientsService)
        {
            _logger = logger;
            _ingredientsService = ingredientsService;
        }

        public async Task Handle(IntegrationContext<IngredientsGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var count = await _ingredientsService.Count();

            await msgContext.RespondAsync(count);
        }
    }
}