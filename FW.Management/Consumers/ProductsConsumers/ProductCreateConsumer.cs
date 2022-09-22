using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts;

/// <summary>
/// Консьюмер для создания Продукта
/// </summary>
namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductCreateConsumer : IConsumer<ProductCreateDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductCreateConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductCreateDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var productId = await _productsService.Create(msgContext.Message);

            if (productId != Guid.Empty)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = productId,
                    Status = StatusResult.Ok,
                    Title = "Added"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = Guid.Empty,
                    Status = StatusResult.Error,
                    Title = "Error"
                });
            }
        }
    }
}
