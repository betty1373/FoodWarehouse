using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.Dishes
{
    public class DishUpdateEventHandler : IIntegrationEventHandler<DishUpdateDto>
    {
        private readonly ILogger _logger;
        private readonly IDishesService _dishesService;

        public DishUpdateEventHandler(ILogger logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        public async Task Handle(IntegrationContext<DishUpdateDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var dishDto = msgContext.Message;
            var status = await _dishesService.Update(dishDto);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = dishDto.Id,
                    Status = StatusResult.Ok,
                    Title = "Updated"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
