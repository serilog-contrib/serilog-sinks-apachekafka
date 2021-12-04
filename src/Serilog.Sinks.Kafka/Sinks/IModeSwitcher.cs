using System;
using Confluent.Kafka;

namespace Serilog.Sinks.Kafka.Sinks
{
    internal interface IModeSwitcher
    {
        Mode CurrentMode { get; }

        void SwitchToFallback(Exception exceptionReason);

        void SwitchToFallback(Error reason);
    }
}
