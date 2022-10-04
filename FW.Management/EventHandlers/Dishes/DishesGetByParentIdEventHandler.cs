using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishesGetByParentIdEventHandler : IIntegrationEventHandler<DishesGetByParentIdDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _service;

        public DishesGetByParentIdEventHandler(ILogger logger, IDishesService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Handle(IntegrationContext<DishesGetByParentIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var userId = msgContext.Message.UserId;
            var dishesDto = await _service.GetByParentId(userId);

            await msgContext.RespondAsync(new DishesResponseDto { Dishes = dishesDto });
        }
    }
}
