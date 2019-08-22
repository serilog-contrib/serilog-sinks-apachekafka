using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Serilog.Sinks.Kafka.Tests.TestData
{
    [ExcludeFromCodeCoverage]
    public class NonNegativeTimeSpanTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                TimeSpan.Zero
            };

            yield return new object[]
            {
                TimeSpan.FromTicks(1)
            };

            yield return new object[]
            {
                TimeSpan.FromMilliseconds(1)
            };

            yield return new object[]
            {
                TimeSpan.FromSeconds(1)
            };

            yield return new object[]
            {
                TimeSpan.FromMinutes(1)
            };

            yield return new object[]
            {
                TimeSpan.FromHours(1)
            };

            yield return new object[]
            {
                TimeSpan.FromDays(1)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
