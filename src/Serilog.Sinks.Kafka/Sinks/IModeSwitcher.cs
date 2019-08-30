using System;
using Confluent.Kafka;

namespace Serilog.Sinks.Kafka.Sinks
{
    internal interface IModeSwitcher
    {
        Mode CurrentMode { get; }

        void SwitchToFailover(Exception exceptionReason);
        
        void SwitchToFailover(Error reason);
    }
}
