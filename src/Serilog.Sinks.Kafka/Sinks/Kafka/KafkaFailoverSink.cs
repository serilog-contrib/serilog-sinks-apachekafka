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
        private readonly KafkaSink _kafkaSink;
        private readonly ILogEventSink _failoverSink;
        private readonly IModeSwitcher _switcher;

        internal static KafkaFailoverSink Create(KafkaSink kafkaSink, ILogEventSink failoverSink,
            BatchOptions batchOptions)
        {
            return batchOptions.QueueLimit.HasValue
                ? new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period,
                    batchOptions.QueueLimit.Value)
                : new KafkaFailoverSink(kafkaSink, failoverSink, batchOptions.BatchSizeLimit, batchOptions.Period);
        }

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit,
            TimeSpan period) :
            base(batchSizeLimit, period)
        {
            _kafkaSink = kafkaSink;
            _failoverSink = failoverSink;
        }

        private KafkaFailoverSink(KafkaSink kafkaSink, ILogEventSink failoverSink, int batchSizeLimit, TimeSpan period,
            int queueLimit) : base(batchSizeLimit, period, queueLimit)
        {
            _kafkaSink = kafkaSink;
            _failoverSink = failoverSink;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            if (_switcher.CurrentMode == Mode.Failover)
            {

                foreach (var logEvent in events)
                {
                    _failoverSink.Emit(logEvent);
                }
                return;
            }

            try
            {
                await _kafkaSink.LogEntriesAsync(events);
            }
            catch (Exception ex)
            {
                _switcher.SwitchToFailover(ex);

                foreach (var logEvent in events)
                {
                    _failoverSink.Emit(logEvent);
                }
            }    
        }
    }
}