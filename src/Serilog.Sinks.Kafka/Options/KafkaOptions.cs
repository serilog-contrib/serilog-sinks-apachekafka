using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// setters are used during deserialization from configuration file

namespace Serilog.Sinks.Kafka.Options
{
    /// <summary>
    ///     The options to configure communication with kafka.
    /// </summary>
    public class KafkaOptions
    {
        private List<string> _brokers;
        private ProducerOptions _producer = new ProducerOptions();

        private string _topicName;

        /// <inheritdoc />
        /// <summary>
        ///     Constructs <see cref="T:Adform.LaaS.Options.KafkaOptions" />.
        /// </summary>
        /// <param name="brokers">The list of kafka brokers.</param>
        /// <param name="topicName">The name of kafka topic.</param>
        /// <param name="producerOptions">The options to configure kafka producer.</param>
        public KafkaOptions(List<string> brokers, string topicName, ProducerOptions producerOptions)
            : this(brokers, topicName)
        {
            Producer = producerOptions;
        }

        /// <summary>
        ///     Constructs <see cref="KafkaOptions" />.
        /// </summary>
        /// <param name="brokers">The list of kafka brokers.</param>
        /// <param name="topicName">The name of kafka topic.</param>
        public KafkaOptions(List<string> brokers, string topicName)
        {
            Brokers = brokers;
            TopicName = topicName;
        }

        /// <summary>
        ///     Constructs <see cref="KafkaOptions" />.
        /// </summary>
        /// <remarks>This constructor is used during deserialization from configuration file</remarks>
        public KafkaOptions()
        {
        }

        /// <summary>
        ///     The list of kafka brokers.
        /// </summary>
        /// <exception cref="ArgumentNullException">Value is <code>null</code>.</exception>
        /// <exception cref="ArgumentException">Any item from value is <code>null</code> or whitespace.</exception>
        public List<string> Brokers
        {
            get => _brokers;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (value.Any(string.IsNullOrWhiteSpace))
                    throw new ArgumentException("The parameter must be specified with non-whitespace strings",
                        nameof(value));

                _brokers = new List<string>(value);
            }
        }

        /// <summary>
        ///     The name of kafka topic.
        /// </summary>
        /// <exception cref="ArgumentException">Value is <code>null</code> or whitespace.</exception>
        public string TopicName
        {
            get => _topicName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value should not be null or whitespace", nameof(value));

                _topicName = value;
            }
        }

        /// <summary>
        ///     The options to configure kafka producer.
        /// </summary>
        /// <exception cref="ArgumentNullException">Value is <code>null</code>.</exception>
        public ProducerOptions Producer
        {
            get => _producer;
            set => _producer = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
