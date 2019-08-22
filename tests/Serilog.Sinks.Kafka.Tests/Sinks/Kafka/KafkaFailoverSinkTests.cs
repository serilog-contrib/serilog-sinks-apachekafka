using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Moq.Protected;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Sinks;
using Serilog.Sinks.Kafka.Sinks.Kafka;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Sinks.Kafka
{
    [ExcludeFromCodeCoverage]
    public class KafkaFailoverSinkTests
    {
        public KafkaFailoverSinkTests()
        {
            _kafkaSinkMock = new Mock<KafkaSink>();
            _failoverMock = new Mock<ILogEventSink>();
            _modeSwitcherMock = new Mock<IModeSwitcher>();

            _failoverSink = KafkaFailoverSink.Create(_kafkaSinkMock.Object, _failoverMock.Object, new BatchOptions(),
                _modeSwitcherMock.Object);

            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        private readonly KafkaFailoverSink _failoverSink;

        private readonly Mock<KafkaSink> _kafkaSinkMock;
        private readonly Mock<ILogEventSink> _failoverMock;
        private readonly Mock<IModeSwitcher> _modeSwitcherMock;

        private readonly Fixture _fixture;

        [Fact]
        public async Task EmitBatchAsync_()
        {
            // Arrange
            const int logEventCount = 10;
            var logEvents = _fixture.CreateMany<LogEvent>(logEventCount);

            _kafkaSinkMock.Protected().Setup<Task>("EmitBatchAsync", ItExpr.IsAny<IEnumerable<LogEvent>>())
                .ThrowsAsync(new TaskCanceledException());

            // Act
            await _failoverSink.EmitBatchImmediatelyAsync(logEvents);

            // Assert
            _kafkaSinkMock.Protected()
                .Verify<Task>("EmitBatchAsync", Times.Once(), ItExpr.IsAny<IEnumerable<LogEvent>>());
            _failoverMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Exactly(10));
        }

        [Fact]
        public async Task EmitBatchAsync_ShouldEmitFailoverSink_WhenCurrentModeIsFailover()
        {
            // Arrange
            const int logEventCount = 10;
            var logEvents = _fixture.CreateMany<LogEvent>(logEventCount);

            _modeSwitcherMock.Setup(x => x.CurrentMode)
                .Returns(() => Mode.Failover);

            // Act
            await _failoverSink.EmitBatchImmediatelyAsync(logEvents);

            // Assert
            _kafkaSinkMock.Protected()
                .Verify<Task>("EmitBatchAsync", Times.Never(), ItExpr.IsAny<IEnumerable<LogEvent>>());
            _failoverMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Exactly(logEventCount));
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
            await _failoverSink.EmitBatchImmediatelyAsync(logEvents);

            // Assert
            _kafkaSinkMock.Protected()
                .Verify<Task>("EmitBatchAsync", Times.Once(), ItExpr.IsAny<IEnumerable<LogEvent>>());
            _failoverMock.Verify(x => x.Emit(It.IsAny<LogEvent>()), Times.Never);
        }
    }
}
