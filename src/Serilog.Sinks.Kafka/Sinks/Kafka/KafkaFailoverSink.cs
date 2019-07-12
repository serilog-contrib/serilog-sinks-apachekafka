using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    public class KafkaFailoverSink : PeriodicBatchingSink
    {
        public KafkaFailoverSink(int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period)
        {
        }

        public KafkaFailoverSink(int batchSizeLimit, TimeSpan period, int queueLimit) : base(batchSizeLimit, period,
            queueLimit)
        {
        }

        protected override Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            return Task.CompletedTask;
//            if (Switcher.CurrentMode == Mode.Failover)
//            {
//                await FailoverLogger.LogEntryAsync(entries);
//                return;
//            }
//
//            try
//            {
//                await PrimaryLogger.LogEntryAsync(entries);
//            }
//            catch (Exception ex)
//            {
//                Switcher.SwitchToFailover(ex);
//
//                await FailoverLogger.LogEntryAsync(entries);
//            }    
        }
    }
}