using MassTransit;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductsGetCountConsumer : IConsumer<ProductsGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductsGetCountConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductsGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var count = await _productsService.Count();

            await msgContext.RespondAsync(new CountResponseDto
            {
                Count = count
            });
        }
    }
}
