using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishCreateEventHandler : IIntegrationEventHandler<DishCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishCreateEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishCreateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishId = await _dishesService.Create(msgContext.Message);

            if (dishId != Guid.Empty)
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = dishId,
                    Status = StatusResult.Ok,
                    Title = "Added"
                });
            else
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.Error,
                    Title = "Error"
                });
        }
    }
}
