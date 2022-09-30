using MassTransit;
using FW.BusinessLogic.Contracts.Products;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.ProductsConsumers
{
    public class ProductsGetByParentIdConsumer : IConsumer<ProductsGetByParentIdDto>
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;

        public ProductsGetByParentIdConsumer(ILogger logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductsGetByParentIdDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");
            var parentId = msgContext.Message.WarehauseId;
            var productsDto = await _productsService.GetByParentId(parentId);
            
            await msgContext.RespondAsync<ProductsResponseDto>(new ProductsResponseDto
            { 
                Products = productsDto 
            });
        }
    }
}
