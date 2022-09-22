using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishGetByIdEventHandler : IIntegrationEventHandler<DishGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishGetByIdEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishId = msgContext.Message.Id;
            var dishDto = await _dishesService.GetById(dishId);

            if (dishDto != null)
                await msgContext.RespondAsync(dishDto);
            else
                await msgContext.RespondAsync(new DishResponseDto{});
        }
    }
}
