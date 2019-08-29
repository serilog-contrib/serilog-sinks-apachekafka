using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Bogus.DataSets;

namespace Serilog.Sinks.Kafka.Benchmarks
{
    public class StringWriterPoolBenchmarks
    {
        [Params(1, 10)]
        public int Amount;
        
        public int CharactersLimit = 5000;

        // [Params(500, 1500, 2500)]
        public int InitialCharactersAmount = 500;

        // [Params(50, 100, 150)]
        public int MessageAmount = 50;

        private StringWriterPool _pool;
        private string[] _text;

        [GlobalSetup]
        public void Setup()
        {
            var lorem = new Lorem();
            _pool = new StringWriterPool(Amount, InitialCharactersAmount, CharactersLimit);
            _text = Enumerable.Range(0, MessageAmount)
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
                    return holder.Object.WriteAsync(x);
                }
            }));
        }
    }
}
