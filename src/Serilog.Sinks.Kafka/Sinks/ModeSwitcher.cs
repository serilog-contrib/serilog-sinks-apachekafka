using System;
using Confluent.Kafka;
using Serilog.Debugging;

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
                throw new ArgumentOutOfRangeException(nameof(fallbackTime), "The parameter must be positive");

            _timeToSwitchToPrimary = DateTime.UtcNow;
            _fallbackTime = fallbackTime;
        }

        public Mode CurrentMode
        {
            get
            {
                if (_currentMode == Mode.Fallback && _timeToSwitchToPrimary <= DateTime.UtcNow)
                {
                    _currentMode = Mode.Primary;
                    SelfLog.WriteLine("Switched to primary.");
                }

                return _currentMode;
            }
        }

        public void SwitchToFallback(Exception exceptionReason)
        {
            _currentMode = Mode.Fallback;
            _timeToSwitchToPrimary = DateTime.UtcNow.Add(_fallbackTime);

            SelfLog.WriteLine("Switched to fallback due to {0}. Exception: {1}.",
                exceptionReason.Message, exceptionReason);
        }

        public void SwitchToFallback(Error reason)
        {
            _currentMode = Mode.Fallback;
            _timeToSwitchToPrimary = DateTime.UtcNow.Add(_fallbackTime);

            SelfLog.WriteLine("Switched to fallback due to {0}.", reason);
        }
    }
}
