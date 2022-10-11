using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.BusinessLogic.Contracts.Recipes;
using FW.BusinessLogic.Services.Abstractions;
using FW.EventBus;
using FW.EventBus.Interfaces;

namespace FW.Management.EventHandlers.ChangesProducts
{
    public class ChangesProductsGetByParentIdEventHandler : IIntegrationEventHandler<ChangesProductsGetByParentIdDto>
    {
        private readonly ILogger _logger;
        private readonly IRecipesService _recipesService;

        public ChangesProductsGetByParentIdEventHandler(ILogger logger, IRecipesService recipesService)
        {
            _logger = logger;
            _recipesService = recipesService;
        }

        public async Task Handle(IntegrationContext<ChangesProductsGetByParentIdDto> msgContext)
        {
            _logger.Information($"Received a message from exchange/queue: {msgContext.ExchangeName}/{msgContext.QueueName}");

            var parentId = msgContext.Message.ProductId;
            var recipesDto = await _recipesService.GetByParentId(parentId);

            await msgContext.RespondAsync(new RecipesResponseDto { Recipes = recipesDto });
        }
    }
}
