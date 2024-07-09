using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;
using System.Text.Json.Nodes;
using static Confluent.Kafka.ConfigPropertyNames;

namespace RedditMetrics.Business.KafkaWrappers
{
    public class ConsumerWrapper : IConsumerWrapper
    {       
        private readonly ConsumerConfig? _consumerConfig;
        private readonly ConsumerBuilder<string, string> _consumerBuilder;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger _logger;
        private readonly IBucketManager _manager;
        private bool disposedValue;
        public ConsumerWrapper(IBucketManager bucketManager, ConsumerConfig? config, ILogger logger)
        {           
            _consumerConfig = config;
            _logger = logger;
            _consumerBuilder = new ConsumerBuilder<string, string>(_consumerConfig);
            _consumer = _consumerBuilder.Build();
            _consumer.Subscribe((_manager = bucketManager).Topic);
        }
        public async Task ExecuteAsync(CancellationToken token)
        {
            _logger.LogInformation(@"Starting listening to topic {topic}", _manager.Topic);
        
            await Task.Yield();   

            while (!token.IsCancellationRequested) {
               
                var consumeResult = _consumer.Consume(token);
                ISubredditResult? subredditResult;

                _logger.LogInformation(@"Consuming data for reddit {topic}", _manager.Topic);
                if ((subredditResult = BuildResults(consumeResult.Message.Value)) != null)
                {
                    await _manager.PopulateBucket(_logger, subredditResult);
                }
                _consumer.Commit(consumeResult);
            }
      
        }

        private SubredditResult? BuildResults(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<SubredditResult>(data);
            }
            catch (Exception x)
            {
                _logger.LogError(@"Kafka data cannot be converted to Reddit result data: {error} ", x.Message);
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { _manager.Dispose(); _consumer?.Close(); _consumer?.Dispose(); }                               
                disposedValue = true;
            }
        }      
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
