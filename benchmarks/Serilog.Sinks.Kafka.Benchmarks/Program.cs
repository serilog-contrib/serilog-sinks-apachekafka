using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using Microsoft.Diagnostics.Tracing.Analysis;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .AddJob(Job.Default.WithRuntime(ClrRuntime.Net472))
                //.With(Job.Default.With(Runtime.Core).With(CsProjCoreToolchain.NetCoreApp20))
                //.With(Job.Default.With(Runtime.Core).With(CsProjCoreToolchain.NetCoreApp21))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core31))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core50))
                .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60));
            //.With(Job.Default.With(Runtime.Core).With(CsProjCoreToolchain.NetCoreApp30))

            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, config);
        }
    }
}
