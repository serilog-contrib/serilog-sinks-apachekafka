using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog.Debugging;
using Serilog.Formatting.Json;
using Serilog.Sinks.Kafka.Options;

[assembly: ExcludeFromCodeCoverage]

namespace Serilog.Sinks.Kafka.Examples.NetCore.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Kafka(
                    new JsonFormatter(), new KafkaOptions(new List<string> {"kafka:9092"}, "console-example"),
                    new BatchOptions())
                .Enrich.FromLogContext()
                .CreateLogger();

            Log.Information("Hello World!");

            SelfLog.Enable(message => System.Console.WriteLine(message));
            
            Log.CloseAndFlush();
        }
    }
}
