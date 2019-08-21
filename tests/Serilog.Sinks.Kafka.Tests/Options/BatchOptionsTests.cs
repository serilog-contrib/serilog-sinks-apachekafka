using System;
using System.Diagnostics.CodeAnalysis;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Tests.TestData;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Options
{
    [ExcludeFromCodeCoverage]
    public class BatchOptionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void BatchSizeLimitSetter_ShouldThrowException_WhenValueIsNonPositive(int value)
        {
            // Arrange
            var options = new BatchOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.BatchSizeLimit = value);
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
        public void BatchSizeLimitSetter_ShouldNotThrowException_WhenValueIsPositive(int value)
        {
            // Arrange
            var options = new BatchOptions();
            
            // Act
            options.BatchSizeLimit = value;

            // Assert
            Assert.Equal(value, options.BatchSizeLimit);
        }
        
        [Theory]
        [ClassData(typeof(NegativeTimeSpanTestData))]
        public void PeriodSetter_ShouldThrowException_WhenValueIsNegative(TimeSpan value)
        {
            // Arrange
            var options = new BatchOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.Period = value);
        }

        [Theory]
        [ClassData(typeof(NonNegativeTimeSpanTestData))]
        public void PeriodSetter_ShouldNotThrowException_WhenValueIsNotNegative(TimeSpan value)
        {
            // Arrange
            var options = new BatchOptions();
            
            // Act
            options.Period = value;

            // Assert
            Assert.Equal(value, options.Period);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void QueueLimitSetter_ShouldThrowException_WhenValueIsNonPositive(int value)
        {
            // Arrange
            var options = new BatchOptions();

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.QueueLimit = value);
        }

        [Theory]
        [InlineData(null)]
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
        public void QueueLimitSetter_ShouldNotThrowException_WhenValueIsPositive(int? value)
        {
            // Arrange
            var options = new BatchOptions();
            
            // Act
            options.QueueLimit = value;

            // Assert
            Assert.Equal(value, options.QueueLimit);
        }
    }
}
