using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka
{
    /// <summary>
    /// </summary>
    public static class LoggingConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="formatter"></param>
        /// <param name="kafka"></param>
        /// <param name="failover"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, ILogEventSink failover, TimeSpan fallback)
            => sinkConfiguration.Kafka(formatter, kafka, new BatchOptions(), failover, fallback);

        /// <summary>
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="formatter">Custom formatter.</param>
        /// <param name="kafka"></param>
        /// <param name="batch"></param>
        /// <param name="fallbackTime"></param>
        /// <param name="failover"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, BatchOptions batch, ILogEventSink failover,
            TimeSpan fallbackTime)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            if (kafka == null) throw new ArgumentNullException(nameof(kafka));

            if (batch == null) throw new ArgumentNullException(nameof(batch));

            if (failover == null) throw new ArgumentNullException(nameof(failover));

            if (fallbackTime <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(fallbackTime), "The fallback time must be positive");

            var kafkaSink = KafkaSink.Create(formatter, kafka, batch);
            return sinkConfiguration.Sink(KafkaFailoverSink.Create(kafkaSink, failover, batch, fallbackTime));
        }

        /// <summary>
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="formatter"></param>
        /// <param name="kafka"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka)
            => sinkConfiguration.Kafka(formatter, kafka, new BatchOptions());

        /// <summary>
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="formatter"></param>
        /// <param name="kafka"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, BatchOptions batch)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            if (kafka == null) throw new ArgumentNullException(nameof(kafka));

            if (batch == null) throw new ArgumentNullException(nameof(batch));

            var kafkaSink = KafkaSink.Create(formatter, kafka, batch);

            return sinkConfiguration.Sink(kafkaSink);
        }
    }
}
