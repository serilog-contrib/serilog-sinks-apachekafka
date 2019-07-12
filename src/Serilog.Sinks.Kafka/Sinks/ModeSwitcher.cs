using System;
using Confluent.Kafka;

namespace Serilog.Sinks.Kafka.Sinks
{
    internal sealed class ModeSwitcher : IModeSwitcher
    {
        private readonly TimeSpan _fallbackTime;
        private Mode _currentMode;
        private DateTime _timeToSwitchToPrimary;

        public ModeSwitcher(TimeSpan fallbackTime)
        {
            if (fallbackTime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(fallbackTime), "The parameter must be positive");
            }

            _timeToSwitchToPrimary = DateTime.UtcNow;
            _fallbackTime = fallbackTime;
        }

        public Mode CurrentMode
        {
            get
            {
                if (_currentMode == Mode.Failover && _timeToSwitchToPrimary <= DateTime.UtcNow)
                {
                    _currentMode = Mode.Primary;
                    Debugging.SelfLog.WriteLine("Switched to primary.");
                }

                return _currentMode;
            }
        }

        public void SwitchToFailover(Exception exceptionReason)
        {
            _currentMode = Mode.Failover;
            _timeToSwitchToPrimary = DateTime.UtcNow.Add(_fallbackTime);

            Debugging.SelfLog.WriteLine("Switched to failover due to {0}. Exception: {1}.",
                exceptionReason.Message, exceptionReason);
        }

        public void SwitchToFailover(Error reason)
        {
            _currentMode = Mode.Failover;
            _timeToSwitchToPrimary = DateTime.UtcNow.Add(_fallbackTime);

            Debugging.SelfLog.WriteLine("Switched to failover due to {0}.", reason);
        }
    }
}