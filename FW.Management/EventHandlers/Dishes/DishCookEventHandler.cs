using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishCookEventHandler : IIntegrationEventHandler<DishCookDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishCookEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishCookDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishId = msgContext.Message.Id;
            var userId = msgContext.Message.UserId;
            var numPortions = msgContext.Message.NumPortions;

            var status = await _dishesService.Cook(dishId, userId, numPortions);

            await msgContext.RespondAsync(status);

        }
    }
}
