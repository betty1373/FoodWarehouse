using MassTransit;
using FW.BusinessLogic.Contracts;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoriesGetCountConsumer : IConsumer<CategoriesGetCountDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoriesGetCountConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoriesGetCountDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var count = await _categoriesService.Count();

            await msgContext.RespondAsync<CountResponseDto>(new CountResponseDto
            { 
                Count = count
            });
        }
    }
}