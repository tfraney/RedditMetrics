using Confluent.Kafka;
using RedditMetrics.Business.KafkaWrappers;
using RedditMetricsSmartReporter.Managers;

namespace RedditMetricsSmartReporter.Service.Wrappers
{
    public class HotConsumerWrapper(HotBucketManager manager,  ConsumerConfig config,
                                   ILogger<HotConsumerWrapper> logger) : ConsumerWrapper(manager, config, logger)
    {
    }
    public class NewConsumerWrapper(NewBucketManager manager, ConsumerConfig config, 
                                   ILogger<NewConsumerWrapper> logger) : ConsumerWrapper(manager, config, logger)
    {
    }
    public class TopConsumerWrapper(TopBucketManager manager, ConsumerConfig config, 
                                    ILogger<TopConsumerWrapper> logger) : ConsumerWrapper(manager, config, logger)
    {
    }
}
