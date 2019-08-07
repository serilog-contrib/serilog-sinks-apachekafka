using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// setters are used during deserialization from configuration file

namespace Serilog.Sinks.Kafka.Options
{
    /// <summary>
    ///     The options to configure periodic batching queue
    /// </summary>
    public class BatchOptions
    {
        private int _batchSizeLimit;
        private TimeSpan _period;
        private int? _queueLimit;

        /// <summary>
        ///     Constructs <see cref="BatchOptions" />.
        /// </summary>
        /// <param name="batchSizeLimit">The maximum number of events to include in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="queueLimit">Maximum number of events in the queue.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="batchSizeLimit" /> or <paramref name="period" />
        ///     isn't positive.
        /// </exception>
        public BatchOptions(int batchSizeLimit, TimeSpan period, int? queueLimit)
        {
            BatchSizeLimit = batchSizeLimit;
            Period = period;
            QueueLimit = queueLimit;
        }

        /// <summary>
        ///     Constructs <see cref="BatchOptions" />.
        /// </summary>
        /// <remarks>This constructor is used during deserialization from configuration file</remarks>
        public BatchOptions()
        {
            BatchSizeLimit = 50;
            Period = TimeSpan.FromSeconds(5);
            QueueLimit = null;
        }

        /// <summary>
        ///     The maximum number of events to include in a single batch.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public int BatchSizeLimit
        {
            get => _batchSizeLimit;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The batch size limit must be more than 0");

                _batchSizeLimit = value;
            }
        }

        /// <summary>
        ///     The time to wait between checking for event batches.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> isn't positive.</exception>
        public TimeSpan Period
        {
            get => _period;
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The batching period must be a non-negative timespan");

                _period = value;
            }
        }

        /// <summary>
        ///     Maximum number of events in the queue.
        ///     null for unbounded queue.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" />not null and negative or zero</exception>
        public int? QueueLimit
        {
            get => _queueLimit;
            set
            {
                if (value.HasValue && value.Value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The queue limit must be more than 0. Or equals NULL for non bounded queue");

                _queueLimit = value;
            }
        }
    }
}