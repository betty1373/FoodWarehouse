using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;
using FW.ResponseStatus;

/// <summary>
/// Консьюмер для удаления Продукта
/// </summary>
namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductDeleteConsumer : IConsumer<ProductDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductDeleteConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var productId = msgContext.Message.Id;
            var status = await _productsService.Delete(productId);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = productId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = productId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}
