using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Confluent.Kafka;
using Serilog.Sinks.Kafka.Sinks;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Sinks
{
    [ExcludeFromCodeCoverage]
    public class ModeSwitcherTests
    {
        public static IEnumerable<object[]> CurrentModeTestData
        {
            get
            {
                yield return new object[]
                    {new Action<ModeSwitcher>(switcher => switcher.SwitchToFallback(new Exception()))};
                yield return new object[]
                {
                    new Action<ModeSwitcher>(switcher =>
                        switcher.SwitchToFallback(new Error(ErrorCode.NetworkException)))
                };
            }
        }

        [Theory]
        [MemberData(nameof(CurrentModeTestData))]
        internal void CurrentMode_ShouldSwitchModeToPrimary_AfterFallbackTime(Action<ModeSwitcher> action)
        {
            // Arrange
            var fallbackTime = TimeSpan.FromSeconds(5);

            var switcher = new ModeSwitcher(fallbackTime);

            // Act
            action.Invoke(switcher);

            // Assert
            Assert.Equal(Mode.Fallback, switcher.CurrentMode);
            Thread.Sleep(fallbackTime.Add(TimeSpan.FromSeconds(1)));
            Assert.Equal(Mode.Primary, switcher.CurrentMode);
        }

        [Fact]
        public void Constructor_ShouldThrowsException_WhenFallbackTimeIsLessOrEqualZero()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ModeSwitcher(TimeSpan.Zero));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ModeSwitcher(TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(1))));
        }
    }
}
