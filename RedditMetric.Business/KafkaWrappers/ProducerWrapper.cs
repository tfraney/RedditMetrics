using Confluent.Kafka;
using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetrics.Business.KafkaWrappers
{
    public class ProducerWrapper : IProducerWrapper
    {
        
        private readonly ProducerBuilder<string, string>? _producerBuilder;
        private readonly IProducer<string, string>? _producer;
        private readonly ProducerConfig? _config;
        private static readonly Random rand = new();
        private bool disposedValue;

        public ProducerWrapper(ProducerConfig config)
        {           
            _config = config;
            _producerBuilder = new ProducerBuilder<string, string>(_config);
            _producer = _producerBuilder.Build();
        }
        public async Task WriteMessage(string topic, string message)
        {
            if (_producer != null)
            {
                var dr = await _producer.ProduceAsync(topic, new Message<string, string>()
                {
                    Key = rand.Next(5).ToString(),
                    Value = message
                });
                Console.WriteLine($"KAFKA => Delivered large message to '{topic}-{dr.TopicPartitionOffset}' successfully");
            }
            else throw new NotImplementedException(@"Producer not initialized.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) _producer?.Dispose();                
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
