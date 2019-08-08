using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Confluent.Kafka;
using Serilog.Sinks.Kafka.Options;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Options
{
    [ExcludeFromCodeCoverage]
    public class ProducerOptionsTests
    {
        public static IEnumerable<object[]> TimeSpanTestData
        {
            get
            {
                yield return new object[]
                {
                    TimeSpan.Zero
                };

                yield return new object[]
                {
                    TimeSpan.Zero.Subtract(TimeSpan.FromHours(1))
                };
            }
        }

        [Theory]
        [MemberData(nameof(TimeSpanTestData))]
        public void MessageTimeoutSetter_ShouldThrowException_WhenValueIsNotPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MessageTimeout = value);
        }

        [Theory]
        [MemberData(nameof(TimeSpanTestData))]
        public void RetryAfter_ShouldThrowException_WhenValueIsNotPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.RetryAfter = value);
        }

        [Theory]
        [InlineData(-1)]
        public void RetryCount_ShouldThrowException_WhenValueIsNegative(int value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.RetryCount = value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MessageBatchSize_ShouldThrowException_WhenValueIsNotPositive(int value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MessageBatchSize = value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MaxMessagesInBufferingQueue_ShouldThrowException_WhenValueIsNotPositive(int value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MaxMessagesInBufferingQueue = value);
        }

        [Theory]
        [InlineData((CompressionType) 10)]
        public void CompressionType_ShouldThrowException_WhenValueIsNotPositive(CompressionType value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.CompressionType = value);
        }
    }
}