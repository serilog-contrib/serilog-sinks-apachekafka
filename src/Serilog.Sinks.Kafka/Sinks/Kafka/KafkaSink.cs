using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class KafkaSink : PeriodicBatchingSink
    {
        private ITextFormatter _formatter;
        private KafkaProducer _producer;

        /// <remarks>
        ///     Used for calling base constructor for create NonBoundedConcurrentQueue (queueLimit equals -1)
        /// </remarks>
        private KafkaSink(ITextFormatter formatter, KafkaOptions kafkaOptions, int batchSizeLimit, TimeSpan period) :
            base(batchSizeLimit, period)
        {
            Initialize(formatter, kafkaOptions);
        }

        /// <remarks>
        ///     Used for calling base constructor for create BoundedConcurrentQueue
        /// </remarks>
        private KafkaSink(ITextFormatter formatter, KafkaOptions kafkaOptions, BatchOptions batchOptions) :
            base(
                batchOptions.BatchSizeLimit,
                batchOptions.Period,
                batchOptions.QueueLimit ?? throw new ArgumentOutOfRangeException(nameof(batchOptions.QueueLimit),
                    $"QueueLimit cannot be null when calling this {nameof(KafkaSink)} constructor"))
        {
            Initialize(formatter, kafkaOptions);
        }

        public static KafkaSink Create(ITextFormatter formatter, KafkaOptions kafkaOptions, BatchOptions batchOptions)
        {
            return batchOptions.QueueLimit.HasValue
                ? new KafkaSink(formatter, kafkaOptions, batchOptions)
                : new KafkaSink(formatter, kafkaOptions, batchOptions.BatchSizeLimit, batchOptions.Period);
        }

        private void Initialize(ITextFormatter formatter, KafkaOptions kafkaOptions)
        {
            _formatter = formatter;
            _producer = new KafkaProducer(kafkaOptions);

//            _stringWriterPool = new ObjectPool<StringWriter>(60,
//                () => new StringWriter(new StringBuilder(500), CultureInfo.InvariantCulture),
//                writer =>
//                {
//                    var builder = writer.GetStringBuilder();
//                    builder.Length = 0;
//                    if (builder.Capacity > 5_000)
//                    {
//                        builder.Capacity = 5_000;
//                    }
//                });
        }

        protected override Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            //todo: add limitation
            return Task.WhenAll(events.Select(e =>
            {
//                using (var writerHolder = _stringWriterPool.Get())
//                {
//                    _formatter.Format(e, writerHolder.Object);
//                    return _producer.ProduceAsync(writerHolder.Object.ToString());
//                }

                using (var writer = new StringWriter())
                {
                    _formatter.Format(e, writer);
                    return _producer.ProduceAsync(writer.ToString());
                }
            }));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing) _producer?.Dispose();
        }
    }
}