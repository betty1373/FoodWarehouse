using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.ResponseStatus;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoryDeleteConsumer : IConsumer<CategoryDeleteDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoryDeleteConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoryDeleteDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var categoryId = msgContext.Message.Id;
            var status = await _categoriesService.Delete(categoryId);
            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = categoryId,
                    Status = StatusResult.Ok,
                    Title = "Deleted"
                });
            }
            else
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = categoryId,
                    Status = StatusResult.NotFound,
                    Title = "Not found"
                });
            }
        }
    }
}