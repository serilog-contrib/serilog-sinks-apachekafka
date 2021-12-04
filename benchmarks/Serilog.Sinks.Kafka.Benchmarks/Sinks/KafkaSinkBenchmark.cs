using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Parsing;
using Serilog.Sinks.Kafka.Options;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka.Benchmarks.Sinks
{
    [MemoryDiagnoser]
    public class KafkaSinkBenchmark
    {
        private IList<LogEvent> _events;
        private KafkaSink _sink;

        //[Params(1, 50, 500)]
        public int N = 50;

        [Params(1, 10)]
        public int Amount;

        [GlobalSetup]
        public void Setup()
        {
            _sink = new KafkaSink(new JsonFormatter(), new MockKafkaProducer(), new StringWriterPoolOptions { Amount = Amount });

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
