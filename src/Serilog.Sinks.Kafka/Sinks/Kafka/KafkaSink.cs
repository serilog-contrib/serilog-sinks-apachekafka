using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class KafkaSink : IBatchedLogEventSink, IDisposable
    {
        private readonly ITextFormatter _formatter;
        private readonly IKafkaProducer _producer;
        private readonly ObjectPool<StringWriter> _stringWriterPool;

        [Obsolete("Must not be used directly. Only for benchmarks")]
        internal KafkaSink(ITextFormatter formatter, IKafkaProducer producer, StringWriterPoolOptions options)
        {
            _formatter = formatter;
            _producer = producer;
            _stringWriterPool = new StringWriterPool(options.Amount, 500, 5_000);
        }

        /// <remarks>
        ///     Used for calling base constructor for create NonBoundedConcurrentQueue (queueLimit equals -1)
        /// </remarks>
        public KafkaSink(ITextFormatter formatter, KafkaOptions kafkaOptions)
        {
            _formatter = formatter;
            _producer = new KafkaProducer(kafkaOptions);
            _stringWriterPool = new StringWriterPool(10, 500, 5_000);
        }

        public Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var semaphore = new SemaphoreSlim(Environment.ProcessorCount);
            return Task.WhenAll(events.Select(async e =>
            {
                await semaphore.WaitAsync();

                try
                {
                    using var writerHolder = _stringWriterPool.Get();
                    _formatter.Format(e, writerHolder.Object);
                    await _producer.ProduceAsync(writerHolder.Object.ToString());
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        public Task OnEmptyBatchAsync() => Task.CompletedTask;

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }

        public Task LogEntriesAsync(IEnumerable<LogEvent> events) => EmitBatchAsync(events);
    }
}
