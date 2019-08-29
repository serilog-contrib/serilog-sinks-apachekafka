using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Bogus.DataSets;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    public class StringWriterPoolBenchmarks
    {
        [Params(1, 10, 100, 1_000)]
        private int _amount;

        [Params(5000, 6000, 7000, 8000, 9000, 10000)]
        private int _charactersLimit;

        [Params(500, 1500, 2000, 2500)]
        private int _initialCharactersAmonut;

        [Params(50, 100, 150, 200, 250, 300)]
        private int _messageAmount;

        private StringWriterPool _pool;
        private string[] _text;

        [GlobalSetup]
        public void Setup()
        {
            var lorem = new Lorem();
            _pool = new StringWriterPool(_amount, _initialCharactersAmonut, _charactersLimit);
            _text = Enumerable.Range(0, _messageAmount)
                .Select(x => lorem.Sentence())
                .ToArray();
        }

        [Benchmark]
        public async Task Benchmark()
        {
            await Task.WhenAll(_text.Select(x =>
            {
                using (var holder = _pool.Get())
                {
                }
            }));
        }
    }
}
