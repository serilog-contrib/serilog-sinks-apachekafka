using System;
using System.Diagnostics.CodeAnalysis;
using Confluent.Kafka;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Tests.TestData;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Options
{
    [ExcludeFromCodeCoverage]
    public class ProducerOptionsTests
    {
        [Theory]
        [ClassData(typeof(NonPositiveTimeSpanTestData))]
        public void MessageTimeoutSetter_ShouldThrowException_WhenValueIsNotPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MessageTimeout = value);
        }

        [Theory]
        [ClassData(typeof(PositiveTimeSpanTestData))]
        public void MessageTimeoutSetter_shouldNotThrowException_WhenValueIsPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();
            
            // Act
            options.MessageTimeout = value;

            // Assert
            Assert.Equal(value, options.MessageTimeout);
        }
        
        [Theory]
        [ClassData(typeof(NonPositiveTimeSpanTestData))]
        public void RetryAfter_ShouldThrowException_WhenValueIsNotPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.RetryAfter = value);
        }

        [Theory]
        [ClassData(typeof(PositiveTimeSpanTestData))]
        public void RetryAfter_ShouldNotThrowException_WhenValueIsPositive(TimeSpan value)
        {
            // Arrange
            var options = new ProducerOptions();
            
            // Act
            options.RetryAfter = value;
            
            // Arrange
            Assert.Equal(value, options.RetryAfter);
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
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(10_000_000)]
        [InlineData(100_000_000)]
        [InlineData(1_000_000_000)]
        public void RetryCount_ShouldNotThrowException_WhenValueIsPositive(int value)
        {
            // Arrange
            var options = new ProducerOptions();
            
            // Act
            options.RetryCount = value;

            // Assert
            Assert.Equal(value, options.RetryCount);
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
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(10_000_000)]
        [InlineData(100_000_000)]
        [InlineData(1_000_000_000)]
        public void MessageBatchSize_ShouldNotThrowException_WhenValueIsPositive(int value)
        {
            // Arrange
            var options = new ProducerOptions();
            
            // Act
            options.MessageBatchSize = value;

            // Assert
            Assert.Equal(value, options.MessageBatchSize);
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
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(10_000_000)]
        [InlineData(100_000_000)]
        [InlineData(1_000_000_000)]
        public void MaxMessagesInBufferingQueue_ShouldNotThrowException_WhenValueIsPosistive(int value)
        {
            // Arrange
            var options = new ProducerOptions();
            
            // Act
            options.MaxMessagesInBufferingQueue = value;

            // Assert
            Assert.Equal(value, options.MaxMessagesInBufferingQueue);
        }
        
        [Theory]
        [InlineData((CompressionType) 10)]
        public void CompressionType_ShouldThrowException_WhenValueIsNotDefineInEnum(CompressionType value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.CompressionType = value);
        }
        
        [Theory]
        [InlineData(CompressionType.Gzip)]
        [InlineData(CompressionType.Lz4)]
        [InlineData(CompressionType.None)]
        [InlineData(CompressionType.Snappy)]
        [InlineData(CompressionType.Zstd)]
        public void CompressionType_ShouldNotThrowException_WhenValueIsDefineInEnum(CompressionType value)
        {
            // Arrange
            var options = new ProducerOptions();

            // Act
            options.CompressionType = value;
            
            // Assert
            Assert.Equal(value, options.CompressionType);
        }
    }
}