using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductsGetAllConsumer : IConsumer<ProductsGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductsGetAllConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductsGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var productsDto = await _productsService.GetAll();

            await msgContext.RespondAsync(new ProductsResponseDto
            { 
                Products = productsDto 
            });
        }
    }
}
