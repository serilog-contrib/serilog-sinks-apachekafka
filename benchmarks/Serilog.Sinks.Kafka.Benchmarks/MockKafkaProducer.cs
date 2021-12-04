using System;
using System.Threading.Tasks;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    public class MockKafkaProducer : IKafkaProducer
    {
        private readonly TimeSpan _delay = TimeSpan.FromMilliseconds(25);

        // emulate insert message to kafka
        public Task ProduceAsync(string message) => Task.Delay(_delay);

        public void Flush()
        {
        }

        public void Dispose()
        {
        }
    }
}
