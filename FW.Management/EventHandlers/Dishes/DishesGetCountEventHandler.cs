using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts.Dishes;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishesGetCountEventHandler : IIntegrationEventHandler<DishesGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishesGetCountEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishesGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var count = await _dishesService.Count();

            await msgContext.RespondAsync(count);
        }
    }
}