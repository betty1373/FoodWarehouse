using FW.EventBus;
using FW.EventBus.Interfaces;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishesGetAllEventHandler : IIntegrationEventHandler<DishesGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishesGetAllEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishesGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishesDto = await _dishesService.GetAll();

            await msgContext.RespondAsync(new DishesResponseDto 
            {
                Dishes = dishesDto
            });
        }
    }
}