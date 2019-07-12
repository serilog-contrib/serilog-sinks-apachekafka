using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Sinks.Kafka;
using Serilog.Sinks.Kafka.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Sinks.Options;

namespace Serilog.Sinks.Kafka
{
    /// <summary>
    /// </summary>
    public static class LoggingConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="formatter">Custom formatter.</param>
        /// <param name="kafka"></param>
        /// <param name="batch"></param>
        /// <param name="fallback"></param>
        /// <param name="failoverSink"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, BatchOptions batch, ILogEventSink failoverSink,
            TimeSpan fallback)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            if (kafka == null)
                throw new ArgumentNullException(nameof(kafka));

            if (batch == null)
                throw new ArgumentNullException(nameof(batch));

            if (failoverSink == null)
                throw new ArgumentNullException(nameof(failoverSink));

            if (fallback <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(fallback), "The fallback time must be positive");

            var kafkaSink = KafkaSink.Create(formatter, kafka, batch);

            //todo: temporarily
            return sinkConfiguration.Sink(kafkaSink);
        }

        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, BatchOptions batch)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            if (kafka == null)
                throw new ArgumentNullException(nameof(kafka));

            if (batch == null)
                throw new ArgumentNullException(nameof(batch));

            var kafkaSink = KafkaSink.Create(formatter, kafka, batch);

            return sinkConfiguration.Sink(kafkaSink);
        }
    }
}