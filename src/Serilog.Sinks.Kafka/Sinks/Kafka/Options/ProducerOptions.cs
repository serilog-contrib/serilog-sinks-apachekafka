using System;
using Confluent.Kafka;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Serilog.Sinks.Kafka.Sinks.Kafka.Options
{
    /// <summary>
    ///     The options to configure kafka producer
    /// </summary>
    public class ProducerOptions
    {
        private CompressionType _compressionType = CompressionType.Lz4;
        private int _maxMessageInBufferingQueue = 10_000_000;

        private int _messageBatchSize = 50;

        private TimeSpan _messageTimeout = TimeSpan.FromSeconds(3);

        private TimeSpan _retryAfter = TimeSpan.FromSeconds(1);

        private int _retryCount = 5;

        /// <summary>
        ///     Local message timeout. This value is only enforced locally and limits the time a produced message waits
        ///     for successful delivery. The default is <code>TimeSpan.FromSeconds(5)</code>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public TimeSpan MessageTimeout
        {
            get => _messageTimeout;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value), "The argument must be positive");

                _messageTimeout = value;
            }
        }

        /// <summary>
        ///     The back off time before retrying a protocol request. The default is <code>TimeSpan.FromMilliseconds(1)</code>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public TimeSpan RetryAfter
        {
            get => _retryAfter;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value), "The argument must be positive");

                _retryAfter = value;
            }
        }

        /// <summary>
        ///     How many times to retry sending a failing MessageSet. The default is <code>0</code>.
        ///     <remarks>Retrying may cause reordering.</remarks>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is negative.</exception>
        public int RetryCount
        {
            get => _retryCount;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "The argument must be non-negative");

                _retryCount = value;
            }
        }

        /// <summary>
        ///     Maximum number of messages batched in one MessageSet. The default is <code>20 000</code>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public int MessageBatchSize
        {
            get => _messageBatchSize;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "The argument must be positive");

                _messageBatchSize = value;
            }
        }

        /// <summary>
        ///     Maximum number of messages allowed on the producer queue. The default is <code>10 000 000</code>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public int MaxMessagesInBufferingQueue
        {
            get => _maxMessageInBufferingQueue;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "The argument must be positive");

                _maxMessageInBufferingQueue = value;
            }
        }

        /// <summary>
        ///     Compression codec to use for compressing message sets. The default is <code>Lz4</code>.
        /// </summary>
        public CompressionType CompressionType
        {
            get => _compressionType;
            set
            {
                if (!Enum.IsDefined(typeof(CompressionType), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "The argument must be defined enum value.");

                _compressionType = value;
            }
        }
    }
}