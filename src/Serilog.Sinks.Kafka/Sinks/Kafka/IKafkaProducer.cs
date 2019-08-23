using System;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    public interface IKafkaProducer : IDisposable
    {
        Task ProduceAsync(string message);
    }
}
