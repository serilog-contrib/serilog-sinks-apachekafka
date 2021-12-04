using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Sinks.Kafka;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka
{
    /// <summary>Extends <see cref="LoggerSinkConfiguration" /> with methods to add kafka sink.</summary>
    public static class LoggingConfigurationExtensions
    {
        /// <summary>
        ///     Adds a Serilog sink that writes <see cref="Serilog.Events.LogEvent" /> to Apache Kafka using
        ///     a custom <see cref="ITextFormatter" />. In case of kafka unavailability,
        ///     <see cref="Serilog.Events.LogEvent" /> will be written to <paramref name="fallback" /> sink.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">A formatter to convert the log events into text for the kafka.</param>
        /// <param name="kafka">Options to configure communication with kafka.</param>
        /// <param name="fallback">Fallback sink to write the log events when kafka is unavailable.</param>
        /// <param name="fallbackTime">Time to wait between checking of kafka availability.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, ILogEventSink fallback, TimeSpan fallbackTime)
            => sinkConfiguration.Kafka(formatter, kafka, new PeriodicBatchingSinkOptions(), fallback, fallbackTime);

        /// <summary>
        ///     Adds a Serilog sink that writes <see cref="Serilog.Events.LogEvent" /> to Apache Kafka using
        ///     a custom <see cref="ITextFormatter" />. In case of kafka unavailability,
        ///     <see cref="Serilog.Events.LogEvent" /> will be written to <paramref name="fallback" /> sink.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">A formatter to convert the log events into text for the kafka.</param>
        /// <param name="kafka">Options to configure communication with kafka.</param>
        /// <param name="batch">Options to configure sink write log events in batches.</param>
        /// <param name="fallbackTime">Time to wait between checking of kafka availability.</param>
        /// <param name="fallback">Fallback sink to write the log events when kafka is unavailable.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, PeriodicBatchingSinkOptions batch, ILogEventSink fallback,
            TimeSpan fallbackTime)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            if (kafka == null) throw new ArgumentNullException(nameof(kafka));

            if (batch == null) throw new ArgumentNullException(nameof(batch));

            if (fallback == null) throw new ArgumentNullException(nameof(fallback));

            if (fallbackTime <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(fallbackTime), "The fallback time must be positive");

            var kafkaSink = new KafkaSink(formatter, kafka);

            var kafkaFailoverSink = new FailoverSink(kafkaSink, fallback, fallbackTime);
            var batchingSink = new PeriodicBatchingSink(kafkaFailoverSink, batch);

            return sinkConfiguration.Sink(batchingSink);
        }

        /// <summary>
        ///     Adds a Serilog sink that writes <see cref="Serilog.Events.LogEvent" /> to Apache Kafka using
        ///     a custom <see cref="ITextFormatter" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">A formatter to convert the log events into text for the kafka.</param>
        /// <param name="kafka">Options to configure communication with kafka.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka)
            => sinkConfiguration.Kafka(formatter, kafka, new PeriodicBatchingSinkOptions());

        /// <summary>
        ///     Adds a Serilog sink that writes <see cref="Serilog.Events.LogEvent" /> to Apache Kafka using
        ///     a custom <see cref="ITextFormatter" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">A formatter to convert the log events into text for the kafka.</param>
        /// <param name="kafka">Options to configure communication with kafka.</param>
        /// <param name="batch">Options to configure sink write log events in batches.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter, KafkaOptions kafka, PeriodicBatchingSinkOptions batch)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            if (kafka == null) throw new ArgumentNullException(nameof(kafka));

            if (batch == null) throw new ArgumentNullException(nameof(batch));

            var kafkaSink = new KafkaSink(formatter, kafka);

            var batchingSink = new PeriodicBatchingSink(kafkaSink, batch);

            return sinkConfiguration.Sink(batchingSink);
        }
    }
}
