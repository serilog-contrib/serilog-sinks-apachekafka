using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Serilog.Sinks.Kafka.Tests.TestData
{
    [ExcludeFromCodeCoverage]
    public class NonPositiveTimeSpanTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                TimeSpan.Zero
            };

            yield return new object[]
            {
                TimeSpan.Zero.Subtract(TimeSpan.FromTicks(1))
            };

            yield return new object[]
            {
                TimeSpan.Zero.Subtract(TimeSpan.FromMilliseconds(1))
            };

            yield return new object[]
            {
                TimeSpan.Zero.Subtract(TimeSpan.FromSeconds(1))
            };

            yield return new object[]
            {
                TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(1))
            };

            yield return new object[]
            {
                TimeSpan.Zero.Subtract(TimeSpan.FromHours(1))
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
