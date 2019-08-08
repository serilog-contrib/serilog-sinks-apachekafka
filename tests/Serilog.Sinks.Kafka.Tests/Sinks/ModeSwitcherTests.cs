using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Serilog.Sinks.Kafka.Sinks;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Sinks
{
    [ExcludeFromCodeCoverage]
    public class ModeSwitcherTests
    {
        [Fact]
        public void Constructor_ShouldThrowsException_WhenFallbackTimeIsLessOrEqualZero()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ModeSwitcher(TimeSpan.Zero));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ModeSwitcher(TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(1))));
        }

        [Fact]
        public void CurrentMode_ShouldSwitchMode_AfterFallbackTime()
        {
            // Arrange
            var fallbackTime = TimeSpan.FromSeconds(5);

            var switcher = new ModeSwitcher(fallbackTime);

            // Act
            switcher.SwitchToFailover(new Exception());

            // Assert
            Assert.Equal(Mode.Failover, switcher.CurrentMode);
            Thread.Sleep(fallbackTime.Add(TimeSpan.FromSeconds(1)));
            Assert.Equal(Mode.Primary, switcher.CurrentMode);
        }
    }
}
