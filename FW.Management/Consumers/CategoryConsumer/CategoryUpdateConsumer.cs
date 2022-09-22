using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoryUpdateConsumer : IConsumer<CategoryUpdateDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoryUpdateConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoryUpdateDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var categoryDto = msgContext.Message;
            var status = await _categoriesService.Update(categoryDto);

            if (status)
            {
                await msgContext.RespondAsync(new ResponseStatusResult
                {
                    Id = categoryDto.Id,
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
