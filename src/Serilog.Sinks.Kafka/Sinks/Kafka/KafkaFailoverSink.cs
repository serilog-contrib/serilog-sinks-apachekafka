using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class KafkaFailoverSink : PeriodicBatchingSink
    {
        private readonly ILogEventSink _failoverSink;
        private readonly KafkaSink _kafkaSink;
        private readonly IModeSwitcher _switcher;

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit,
            TimeSpan period, IModeSwitcher modeSwitcher) : base(batchSizeLimit, period)
        {
            _kafkaSink = kafkaSink;
            _failoverSink = failoverSink;
            _switcher = modeSwitcher;
        }

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit,
            TimeSpan period, TimeSpan fallback) : this(kafkaSink, failoverSink, batchSizeLimit, period,
            new ModeSwitcher(fallback))
        {
        }

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit, TimeSpan period,
            int queueLimit, IModeSwitcher modeSwitcher) : base(batchSizeLimit, period, queueLimit)
        {
            _kafkaSink = kafkaSink;
            _failoverSink = failoverSink;
            _switcher = modeSwitcher;
        }

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit, TimeSpan period,
            int queueLimit, TimeSpan fallback) : this(kafkaSink, failoverSink, batchSizeLimit, period, queueLimit,
            new ModeSwitcher(fallback))
        {
        }

        internal static KafkaFailoverSink Create(KafkaSink kafkaSink, ILogEventSink failoverSink,
            BatchOptions batchOptions, IModeSwitcher modeSwitcher) =>
            batchOptions.QueueLimit.HasValue
                ? new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period,
                    batchOptions.QueueLimit.Value, modeSwitcher)
                : new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period,
                    modeSwitcher);

        internal static KafkaFailoverSink Create(KafkaSink kafkaSink, ILogEventSink failoverSink,
            BatchOptions batchOptions, TimeSpan fallback) =>
            batchOptions.QueueLimit.HasValue
                ? new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period,
                    batchOptions.QueueLimit.Value, fallback)
                : new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period,
                    fallback);

        // Need only for tests to don't wait timer
        internal Task EmitBatchImmediatelyAsync(IEnumerable<LogEvent> events) => EmitBatchAsync(events);

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            if (_switcher.CurrentMode == Mode.Failover)
            {
                foreach (var logEvent in events) _failoverSink.Emit(logEvent);

                return;
            }

            try
            {
                await _kafkaSink.LogEntriesAsync(events);
            }
            catch (Exception ex)
            {
                _switcher.SwitchToFailover(ex);

                foreach (var logEvent in events) _failoverSink.Emit(logEvent);
            }
        }
    }
}
