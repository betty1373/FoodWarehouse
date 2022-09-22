using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoryGetByIdConsumer : IConsumer<CategoryGetByIdDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoryGetByIdConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoryGetByIdDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var categoryId = msgContext.Message.Id;
            var categoryDto = await _categoriesService.GetById(categoryId);

            if (categoryDto != null)
                await msgContext.RespondAsync(categoryDto);
            else
                await msgContext.RespondAsync(new CategoryResponseDto
                {
                    Name = null,
                   // ModifiedOn = DateTime.MinValue
                });
        }
    }
}
