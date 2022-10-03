using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;

/// <summary>
/// Консьюмер для запроса информации о Продукте по Id
/// </summary>
namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductGetByIdConsumer : IConsumer<ProductGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductGetByIdConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var productId = msgContext.Message.Id;
            var productDto = await _productsService.GetById(productId);

            if (productDto != null)
                await msgContext.RespondAsync(productDto);
            else
                await msgContext.RespondAsync(new ProductResponseDto
                { 
                    //Name = null,
                    Quantity = 0,
                    //ModifiedOn = DateTime.MinValue
                });
        }
    }
}
