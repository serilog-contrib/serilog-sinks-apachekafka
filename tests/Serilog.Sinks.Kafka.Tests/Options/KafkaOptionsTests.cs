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
        [Fact]
        public void BrokersSetter_ShouldThrowsException_WhenValueIsNull()
        {
            // Arrange
            var options = new KafkaOptions();

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => options.Brokers = null);
        }

        public static IEnumerable<object[]> BrokerTestData
        {
            get
            {
                yield return new object[]
                {
                    new List<string>{ "broker", ""}
                };
                
                yield return new object[]
                {
                    new List<string>{ null, "broker"}
                };
                
                yield return new object[]
                {
                    new List<string>{"broker", " "}
                };
            }
        }
        
        [Theory]
        [MemberData(nameof(BrokerTestData))]
        public void BrokersSetter_ShouldThrowsException_WhenAnyValueIsNullOrWhiteSpace(List<string> brokers)
        {
            // Arrange
            var options = new KafkaOptions();
            
            // Act + Assert
            Assert.Throws<ArgumentException>(() => options.Brokers = brokers);
        }

        [Fact]
        public void BrokersSetter_ShouldNotThrowsException_WhenAllValuesAreNotEmptyAndWhiteSpace()
        {
            // Arrange
            var options = new KafkaOptions();
            var brokers = new List<string> {"broker1", "broker2"};
            
            // Act
            options.Brokers = brokers;
            
            // Assert
            Assert.Equal(brokers, options.Brokers);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TopicNameSetter_ShouldThrowsException_WhenValueIsNullOrWhiteSpace(string value)
        {
            // Arrange
            var options = new KafkaOptions();
            
            // Act + Assert
            Assert.Throws<ArgumentException>(() => options.TopicName = value);
        }

        [Fact]
        public void TopicNameSetter_ShouldNotThrowsException_WhenValueIsNotNullOrWhiteSpace()
        {
            // Arrange
            const string topicName = "topic";
            var options = new KafkaOptions();
            
            // Act
            options.TopicName = topicName;
            
            // Assert
            Assert.Equal(topicName, options.TopicName);
        }

        [Fact]
        public void ProducerSetter_ShouldThrowsException_WhenValueIsNull()
        {
            // Arrange
            var options = new KafkaOptions();
            
            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => options.Producer = null);
        }

        [Fact]
        public void ProducerSetter_ShouldNotThrowsException_WhenValueIsNotNull()
        {
            // Arrange
            var options = new KafkaOptions();
            
            // Act
            options.Producer = new ProducerOptions();
            
            // Assert
            Assert.NotNull(options.Producer);
        }
    }
}
