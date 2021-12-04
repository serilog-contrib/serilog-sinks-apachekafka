using BenchmarkDotNet.Configs;
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
                .AddJob(Job.Default.WithRuntime(ClrRuntime.Net472))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core31))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core50))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60));

            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, config);
        }
    }
}
