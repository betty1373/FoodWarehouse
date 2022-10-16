using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.ResponseStatus;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoryCreateConsumer : IConsumer<CategoryCreateDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoryCreateConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoryCreateDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var categoryId = await _categoriesService.Create(msgContext.Message);

            if (categoryId != Guid.Empty)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = categoryId,
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
