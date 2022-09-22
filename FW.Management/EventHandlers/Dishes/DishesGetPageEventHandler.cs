using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishesGetPageEventHandler : IIntegrationEventHandler<DishesGetPageDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishesGetPageEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishesGetPageDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            int page = msgContext.Message.Skip;
            int pageSize = msgContext.Message.Take;

            var dishesDto = await _dishesService.GetPaged(page, pageSize);

            await msgContext.RespondAsync(new DishesResponseDto 
            {
                Dishes = dishesDto
            });
        }
    }
}