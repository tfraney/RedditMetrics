using RedditMetrics.DataLayer.Interfaces;
using RedditMetricsSmartReporter.Service.Wrappers;

namespace RedditMetricsSmartReporter.Service
{


    public class HotRedditMessageConsumer(ILogger<HotRedditMessageConsumer> logger, HotConsumerWrapper wrapper) : RedditMessageConsumer(logger,wrapper) { }
    public class TopRedditMessageConsumer(ILogger<TopRedditMessageConsumer> logger, TopConsumerWrapper wrapper) : RedditMessageConsumer(logger, wrapper) { }
    public class NewRedditMessageConsumer(ILogger<NewRedditMessageConsumer> logger, NewConsumerWrapper wrapper) : RedditMessageConsumer(logger, wrapper) { }

    public abstract class RedditMessageConsumer(ILogger<RedditMessageConsumer> logger, IConsumerWrapper consumeWrapper) : BackgroundService, IDisposable
    {
        private readonly ILogger<RedditMessageConsumer> _logger = logger;
        private readonly IConsumerWrapper _wrapper = consumeWrapper;        

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            _logger.LogInformation(@"Beginning Kafka message consumption.");          
            await _wrapper.ExecuteAsync(token); 
        }

       

    }
}
