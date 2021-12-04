using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Formatting.Json;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Tests.TestData;
using Serilog.Sinks.PeriodicBatching;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests
{
    [ExcludeFromCodeCoverage]
    public class LoggingConfigurationExtensionsTests
    {
        [Theory]
        [ClassData(typeof(NonPositiveTimeSpanTestData))]
        public void KafkaWithFallback_ShouldThrowsException_WhenFallbackTimeIsNonPositive(TimeSpan value)
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");
            var fallbackSink = Mock.Of<ILogEventSink>();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>("fallbackTime",
                () => sinkConfiguration.Kafka(formatter, kafkaOptions, fallbackSink, value));
        }

        [Fact]
        public void KafkaWithFallback_ShouldNotThrowsException_WhenOptionsAreValid()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");
            var fallbackSink = Mock.Of<ILogEventSink>();
            var fallbackTime = TimeSpan.FromMinutes(1);

            // Act
            var sink = sinkConfiguration.Kafka(formatter, kafkaOptions, fallbackSink, fallbackTime);

            // Assert
            Assert.NotNull(sink);
        }

        [Fact]
        public void KafkaWithFallback_ShouldThrowsException_WhenBatchOptionsIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");

            // Act + Assert
            Assert.Throws<ArgumentNullException>("batch",
                () => sinkConfiguration.Kafka(formatter, kafkaOptions, null, null, TimeSpan.Zero));
        }

        [Fact]
        public void KafkaWithFallback_ShouldThrowsException_WhenFallbackIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");

            // Act + Assert
            Assert.Throws<ArgumentNullException>("fallback",
                () => sinkConfiguration.Kafka(formatter, kafkaOptions, null, TimeSpan.Zero));
        }

        [Fact]
        public void KafkaWithFallback_ShouldThrowsException_WhenFormatterIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;

            // Act + Assert
            Assert.Throws<ArgumentNullException>("formatter",
                () => sinkConfiguration.Kafka(null, null, null, TimeSpan.Zero));
        }

        [Fact]
        public void KafkaWithFallback_ShouldThrowsException_WhenKafkaOptionsIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();

            // Act + Assert
            Assert.Throws<ArgumentNullException>("kafka",
                () => sinkConfiguration.Kafka(formatter, null, null, TimeSpan.Zero));
        }

        [Fact]
        public void KafkaWithFallback_ShouldThrowsException_WhenSinkConfigurationIsNull()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentNullException>("sinkConfiguration",
                () => ((LoggerSinkConfiguration)null).Kafka(null, null, null, TimeSpan.Zero));
        }

        [Fact]
        public void KafkaWithoutFallback_ShouldNotThrowsException_WhenAllOptionsAreValid()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");

            // Act
            var sink = sinkConfiguration.Kafka(formatter, kafkaOptions, new PeriodicBatchingSinkOptions());

            // Assert
            Assert.NotNull(sink);
        }

        [Fact]
        public void KafkaWithoutFallback_ShouldThrowsException_WhenBatchOptionsIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();
            var kafkaOptions = new KafkaOptions(new List<string> { "broker" }, "topicName");

            // Act + Assert
            Assert.Throws<ArgumentNullException>("batch",
                () => sinkConfiguration.Kafka(formatter, kafkaOptions, null));
        }

        [Fact]
        public void KafkaWithoutFallback_ShouldThrowsException_WhenFormatterIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;

            // Act + Assert
            Assert.Throws<ArgumentNullException>("formatter",
                () => sinkConfiguration.Kafka(null, null));
        }

        [Fact]
        public void KafkaWithoutFallback_ShouldThrowsException_WhenKafkaOptionsIsNull()
        {
            // Arrange
            var sinkConfiguration = new LoggerConfiguration().WriteTo;
            var formatter = new JsonFormatter();

            // Act + Assert
            Assert.Throws<ArgumentNullException>("kafka",
                () => sinkConfiguration.Kafka(formatter, null));
        }

        [Fact]
        public void KafkaWithoutFallback_ShouldThrowsException_WhenSinkConfigurationIsNull()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentNullException>("sinkConfiguration",
                () => ((LoggerSinkConfiguration)null).Kafka(null, null));
        }
    }
}
