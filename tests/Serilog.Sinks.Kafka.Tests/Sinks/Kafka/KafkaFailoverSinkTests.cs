using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Kafka.Sinks;
using Serilog.Sinks.Kafka.Sinks.Kafka;
using Serilog.Sinks.PeriodicBatching;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Sinks.Kafka
{
    [ExcludeFromCodeCoverage]
    public class KafkaFailoverSinkTests
    {
        private readonly FailoverSink _failoverSink;
        private readonly Mock<ILogEventSink> _fallbackSinkMock;

        private readonly Fixture _fixture;

        private readonly Mock<IBatchedLogEventSink> _kafkaSinkMock;
        private readonly Mock<IModeSwitcher> _modeSwitcherMock;

        public KafkaFailoverSinkTests()
        {
            _kafkaSinkMock = new Mock<IBatchedLogEventSink>();
            _fallbackSinkMock = new Mock<ILogEventSink>();
            _modeSwitcherMock = new Mock<IModeSwitcher>();

            _failoverSink =
                new FailoverSink(_kafkaSinkMock.Object, _fallbackSinkMock.Object, _modeSwitcherMock.Object);

            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task EmitBatchAsync_()
        {
            // Arrange
            const int logEventCount = 10;
            var logEvents = _fixture.CreateMany<LogEvent>(logEventCount);

            _kafkaSinkMock.Setup(x => x.EmitBatchAsync(It.IsAny<IEnumerable<LogEvent>>()))
                .ThrowsAsync(new TaskCanceledException());

            // Act
            await _failoverSink.EmitBatchAsync(logEvents);

            // Assert
            _kafkaSinkMock.Verify(x => x.EmitBatchAsync(It.IsAny<IEnumerable<LogEvent>>()), Times.Once);
            _fallbackSinkMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Exactly(10));
        }

        [Fact]
        public async Task EmitBatchAsync_ShouldEmitFailoverSink_WhenCurrentModeIsFailover()
        {
            // Arrange
            const int logEventCount = 10;
            var logEvents = _fixture.CreateMany<LogEvent>(logEventCount);

            _modeSwitcherMock.Setup(x => x.CurrentMode)
                .Returns(() => Mode.Fallback);

            // Act
            await _failoverSink.EmitBatchAsync(logEvents);

            // Assert
            _kafkaSinkMock.Verify(x => x.EmitBatchAsync(It.IsAny<IEnumerable<LogEvent>>()), Times.Never);
            _fallbackSinkMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Exactly(logEventCount));
        }

        [Fact]
        public async Task EmitBatchAsync_ShouldEmitKafkaSink_WhenCurrentModeIsPrimary()
        {
            // Arrange
            const int logEventCount = 10;
            var logEvents = _fixture.CreateMany<LogEvent>(logEventCount);

            _modeSwitcherMock.Setup(x => x.CurrentMode)
                .Returns(() => Mode.Primary);

            // Act
            await _failoverSink.EmitBatchAsync(logEvents);

            // Assert
            _kafkaSinkMock.Verify(x => x.EmitBatchAsync(It.IsAny<IEnumerable<LogEvent>>()), Times.Once);
            _fallbackSinkMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Never);
        }
    }
}
