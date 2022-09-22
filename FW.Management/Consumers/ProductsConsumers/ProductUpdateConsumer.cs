using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Contracts;

/// <summary>
/// Консьюмер для обновления Продукта
/// </summary>
namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductUpdateConsumer : IConsumer<ProductUpdateDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductUpdateConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductUpdateDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var productDto = msgContext.Message;
            var status = await _productsService.Update(productDto);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = productDto.Id,
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
