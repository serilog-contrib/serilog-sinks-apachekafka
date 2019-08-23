using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog.Sinks.Kafka.Options;
using Xunit;

namespace Serilog.Sinks.Kafka.Tests.Options
{
    [ExcludeFromCodeCoverage]
    public class KafkaOptionsTests
    {
        private readonly KafkaOptions _options;
        
        public KafkaOptionsTests()
        {
            _options = new KafkaOptions(new List<string>{"broker"}, "topicName");    
        }
        
        public static IEnumerable<object[]> BrokerTestData
        {
            get
            {
                yield return new object[]
                {
                    new List<string> {"broker", ""}
                };

                yield return new object[]
                {
                    new List<string> {null, "broker"}
                };

                yield return new object[]
                {
                    new List<string> {"broker", " "}
                };
            }
        }

        [Theory]
        [MemberData(nameof(BrokerTestData))]
        public void BrokersSetter_ShouldThrowsException_WhenAnyValueIsNullOrWhiteSpace(List<string> brokers)
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentException>(() => _options.Brokers = brokers);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TopicNameSetter_ShouldThrowsException_WhenValueIsNullOrWhiteSpace(string value)
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentException>(() => _options.TopicName = value);
        }

        [Fact]
        public void BrokersSetter_ShouldNotThrowsException_WhenAllValuesAreNotEmptyAndWhiteSpace()
        {
            // Arrange
            var brokers = new List<string> {"broker1", "broker2"};

            // Act
            _options.Brokers = brokers;

            // Assert
            Assert.Equal(brokers, _options.Brokers);
        }

        [Fact]
        public void BrokersSetter_ShouldThrowsException_WhenValueIsNull()
        {
            // Arrange+ Act + Assert
            Assert.Throws<ArgumentNullException>(() => _options.Brokers = null);
        }

        [Fact]
        public void ProducerSetter_ShouldNotThrowsException_WhenValueIsNotNull()
        {
            // Arrange + Act
            _options.Producer = new ProducerOptions();

            // Assert
            Assert.NotNull(_options.Producer);
        }

        [Fact]
        public void ProducerSetter_ShouldThrowsException_WhenValueIsNull()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentNullException>(() => _options.Producer = null);
        }

        [Fact]
        public void TopicNameSetter_ShouldNotThrowsException_WhenValueIsNotNullOrWhiteSpace()
        {
            // Arrange
            const string topicName = "topic";

            // Act
            _options.TopicName = topicName;

            // Assert
            Assert.Equal(topicName, _options.TopicName);
        }
    }
}
