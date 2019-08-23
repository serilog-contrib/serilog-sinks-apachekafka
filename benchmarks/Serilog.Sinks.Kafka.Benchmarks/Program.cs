using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .With(Job.Default.With(Runtime.Clr))
                .With(Job.Default.With(Runtime.Core))
                .With(MemoryDiagnoser.Default);
            
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, config);
        }
    }
}
