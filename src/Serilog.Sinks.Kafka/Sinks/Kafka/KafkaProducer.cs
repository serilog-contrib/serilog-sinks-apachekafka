using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Serilog.Sinks.Kafka.Sinks.Kafka.Options;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal sealed class KafkaProducer : IDisposable
    {
        private IProducer<Null, string> Producer { get; }

        private readonly TimeSpan _timeout;
        private readonly string _topicName;
        private bool _disposed;

        public KafkaProducer(KafkaOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var brokers = string.Join(",", options.Brokers);
            _timeout = options.Producer.MessageTimeout;
            _topicName = options.TopicName;
            var producerOptions = options.Producer;
            
            var config = new ProducerConfig
            {
                BootstrapServers = brokers,

                SocketKeepaliveEnable = true,
                SocketNagleDisable = true,

                MessageTimeoutMs = (int) producerOptions.MessageTimeout.TotalMilliseconds,
                QueueBufferingMaxMessages = producerOptions.MaxMessagesInBufferingQueue,
                BatchNumMessages = producerOptions.MessageBatchSize,

                MessageSendMaxRetries = producerOptions.RetryCount,
                RetryBackoffMs = (int) producerOptions.RetryAfter.TotalMilliseconds,

                LogConnectionClose = false,

                CompressionType = producerOptions.CompressionType,
            };
            
            Producer = new ProducerBuilder<Null, string>(config)
                .SetErrorHandler((producer, error) => ProducerOnError(error))
                .Build();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                var producer = Producer;
                producer?.Flush(_timeout);
                producer?.Dispose();
            }
            catch
            {
                // ignore
                // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1065-do-not-raise-exceptions-in-unexpected-locations?view=vs-2019#dispose-methods
            }
            finally
            {
                _disposed = true;  
            }
        }

        private void ProducerOnError(Error error)
        {
            var errorHandler = OnError;
            errorHandler?.Invoke(this, error);
        }

        public Task ProduceAsync(string message)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(KafkaProducer));
            }
            
            return Producer.ProduceAsync(_topicName, new Message<Null, string> {Value = message});
        }

        public void Flush()
        {
            Producer.Flush(_timeout);
        }

        public event EventHandler<Error> OnError;
    }
}