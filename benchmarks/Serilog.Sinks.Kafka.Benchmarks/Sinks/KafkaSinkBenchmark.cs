using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Parsing;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka.Benchmarks.Sinks
{
    [MemoryDiagnoser]
    public class KafkaSinkBenchmark
    {
        private KafkaSink _sink;

        private LogEvent _event;
        private IList<LogEvent> _events;

        [Params(1, 10, 100, 1_000, 10_000)]
        public int N;
        
        [GlobalSetup]
        public void Setup()
        {
            _sink = new KafkaSink(new JsonFormatter(), new MockKafkaProducer());

            _events = Enumerable.Range(0, N)
                .Select(x => new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Information, null,
                    new MessageTemplate("Info text", new List<MessageTemplateToken>()),
                    new List<LogEventProperty>()))
                .ToList();
        }

        [Benchmark]
        public async Task MultipleLogEvents()
        {
            await _sink.LogEntriesAsync(_events);
        }
    }
}
