using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishDeleteEventHandler : IIntegrationEventHandler<DishDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishDeleteEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishId = msgContext.Message.Id;
            var status = await _dishesService.Delete(dishId);
            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = dishId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = dishId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
