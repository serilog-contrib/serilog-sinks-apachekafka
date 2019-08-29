using System.Globalization;
using System.IO;
using System.Text;

namespace Serilog.Sinks.Kafka
{
    internal class StringWriterPool : ObjectPool<StringWriter>
    {
        // todo: add ability to configure such parameters
        internal StringWriterPool(int amount, int initialCharactersAmount, int charactersLimit)
            : base(amount,
                () => new StringWriter(new StringBuilder(initialCharactersAmount), CultureInfo.InvariantCulture),
                writer =>
                {
                    var builder = writer.GetStringBuilder();
                    builder.Length = 0;
                    if (builder.Capacity > charactersLimit) builder.Capacity = charactersLimit;
                })
        {
        }
    }
}
