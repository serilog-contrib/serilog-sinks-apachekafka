using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class KafkaSink : PeriodicBatchingSink
    {
        private readonly ITextFormatter _formatter;
        private readonly IKafkaProducer _producer;
        private readonly ObjectPool<StringWriter> _stringWriterPool;

        // used for mock purposes only
        [Obsolete("Must not be used directly. Only for mock purposes in unit tests")]
        internal KafkaSink() : base(1, TimeSpan.FromSeconds(1))
        {
        }

        [Obsolete("Must not be used directly. Only for benchmarks")]
        internal KafkaSink(ITextFormatter formatter, IKafkaProducer producer, StringWriterPoolOptions options) : this()
        {
            _formatter = formatter;
            _producer = producer;
            _stringWriterPool = new StringWriterPool(options.Amount, 500, 5_000);
        }

        /// <remarks>
        ///     Used for calling base constructor for create NonBoundedConcurrentQueue (queueLimit equals -1)
        /// </remarks>
        private KafkaSink(ITextFormatter formatter, KafkaOptions kafkaOptions, int batchSizeLimit, TimeSpan period) :
            base(batchSizeLimit, period)
        {
            _formatter = formatter;
            _producer = new KafkaProducer(kafkaOptions);
            _stringWriterPool = new StringWriterPool(10, 500, 5_000);
        }

        /// <remarks>
        ///     Used for calling base constructor for create BoundedConcurrentQueue
        /// </remarks>
        private KafkaSink(ITextFormatter formatter, KafkaOptions kafkaOptions, int batchSizeLimit, TimeSpan period,
            int queueLimit) : base(batchSizeLimit, period, queueLimit)
        {
            _formatter = formatter;
            _producer = new KafkaProducer(kafkaOptions);
            _stringWriterPool = new StringWriterPool(10, 500, 5_000);
        }

        public static KafkaSink Create(ITextFormatter formatter, KafkaOptions kafkaOptions,
            BatchOptions batchOptions) =>
            batchOptions.QueueLimit.HasValue
                ? new KafkaSink(formatter, kafkaOptions, batchOptions.BatchSizeLimit, batchOptions.Period,
                    batchOptions.QueueLimit.Value)
                : new KafkaSink(formatter, kafkaOptions, batchOptions.BatchSizeLimit, batchOptions.Period);

        public Task LogEntriesAsync(IEnumerable<LogEvent> events) => EmitBatchAsync(events);

        protected override Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var semaphore = new SemaphoreSlim(Environment.ProcessorCount);
            return Task.WhenAll(events.Select(async e =>
            {
                await semaphore.WaitAsync();
                
                try
                {
                    using (var writerHolder = _stringWriterPool.Get())
                    {
                        _formatter.Format(e, writerHolder.Object);
                        await _producer.ProduceAsync(writerHolder.Object.ToString());
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing || _producer == null) return;
            
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
