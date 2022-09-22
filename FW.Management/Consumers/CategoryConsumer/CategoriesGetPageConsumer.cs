using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Services.Abstractions;


namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoriesGetPageConsumer : IConsumer<CategoriesGetPageDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoriesGetPageConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoriesGetPageDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            int page = msgContext.Message.Skip;
            int pageSize = msgContext.Message.Take;

            var categoriesDto = await _categoriesService.GetPaged(page, pageSize);

            await msgContext.RespondAsync<CategoriesResponseDto>(new CategoriesResponseDto
            { 
                Categories = categoriesDto 
            });
        }
    }
}