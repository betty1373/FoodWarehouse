using MassTransit;
using FW.BusinessLogic.Contracts.Category;
using FW.BusinessLogic.Services.Abstractions;

namespace FW.Management.Consumers.CategoryConsumer
{
    public class CategoriesGetAllConsumer : IConsumer<CategoriesGetAllDto>
    {
        private readonly ILogger _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoriesGetAllConsumer(ILogger logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        public async Task Consume(ConsumeContext<CategoriesGetAllDto> msgContext)
        {
            _logger.Information($"Received a message from the {msgContext.SourceAddress}");

            var categoriesDto = await _categoriesService.GetAll();

            await msgContext.RespondAsync<CategoriesResponseDto>(new CategoriesResponseDto
            {
                Categories = categoriesDto 
            });
        }
    }

    // Пример изменения настроек Consumer
    public class CategoriesGetAllConsumerDefenition : ConsumerDefinition<CategoriesGetAllConsumer>
    {
        public CategoriesGetAllConsumerDefenition()
        {
            ConcurrentMessageLimit = 1;                            // ограничение количества сообщений, потребляемых одновременно
        }

        // изменение конфигурации
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CategoriesGetAllConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
            endpointConfigurator.UseTimeout(x => x.Timeout = TimeSpan.FromSeconds(60));
        }
    }
}