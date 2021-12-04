using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal class FailoverSink : IBatchedLogEventSink
    {
        private readonly ILogEventSink _fallbackSink;
        private readonly IBatchedLogEventSink _primarySink;
        private readonly IModeSwitcher _switcher;

        public FailoverSink(IBatchedLogEventSink primarySink, ILogEventSink fallbackSink,
            IModeSwitcher modeSwitcher)
        {
            _primarySink = primarySink;
            _fallbackSink = fallbackSink;
            _switcher = modeSwitcher;
        }

        public FailoverSink(IBatchedLogEventSink primarySink, ILogEventSink fallbackSink, TimeSpan fallback) : this(
            primarySink, fallbackSink, new ModeSwitcher(fallback))
        {
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            if (_switcher.CurrentMode == Mode.Fallback)
            {
                foreach (var logEvent in events) _fallbackSink.Emit(logEvent);

                return;
            }

            try
            {
                await _primarySink.EmitBatchAsync(events);
            }
            catch (Exception ex)
            {
                _switcher.SwitchToFallback(ex);

                foreach (var logEvent in events) _fallbackSink.Emit(logEvent);
            }
        }

        public Task OnEmptyBatchAsync() => Task.CompletedTask;
    }
}
