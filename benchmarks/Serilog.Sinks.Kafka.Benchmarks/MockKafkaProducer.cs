using System.Threading.Tasks;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    public class MockKafkaProducer : IKafkaProducer
    {
        public Task ProduceAsync(string message) => Task.CompletedTask;

        public void Dispose()
        {
        }
    }
}
