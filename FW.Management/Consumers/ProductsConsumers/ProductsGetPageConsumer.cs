using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductsGetPageConsumer : IConsumer<ProductsGetPageDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductsGetPageConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductsGetPageDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            int page = msgContext.Message.Skip;
            int pageSize = msgContext.Message.Take;

            var productsDto = await _productsService.GetPaged(page, pageSize);

            await msgContext.RespondAsync<ProductsResponseDto>(new{ 
                Products = productsDto });
        }
    }
}
