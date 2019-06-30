using System;
using Serilog.Configuration;
using Serilog.Formatting;
using Serilog.Sinks.Kafka.Sinks.Kafka;

namespace Serilog.Sinks.Kafka
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggingConfigurationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter)
        {
            return sinkConfiguration.Sink(new KafkaSink(50, TimeSpan.FromSeconds(15)));
        }
    }
}
