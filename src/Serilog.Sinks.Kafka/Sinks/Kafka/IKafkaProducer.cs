using System;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka.Sinks.Kafka
{
    internal interface IKafkaProducer : IDisposable
    {
        Task ProduceAsync(string message);

        void Flush();
    }
}
