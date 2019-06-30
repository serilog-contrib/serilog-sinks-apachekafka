using System;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class KafkaSink : PeriodicBatchingSink
    {
        internal KafkaSink(int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period)
        {
        }

        internal KafkaSink(int batchSizeLimit, TimeSpan period, int queueLimit) : base(batchSizeLimit, period, queueLimit)
        {
        }
    }
}
