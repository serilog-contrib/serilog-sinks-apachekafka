# Kafka sink for Serilog

A sink for [Serilog](https://serilog.net/) that writes events to [Kafka](https://kafka.apache.org/) with failover functionality.

## Installation

```powershell
Install-Package Serilog.Sinks.ApacheKafka
```

## Usage

```csharp
var loggerConfig = new LoggerConfiguration()
    .WriteTo.Kafka(
        new JsonFormatter(),
        new KafkaOptions(new List<string> { "kafka:9092" }, "topic"));
```

## Roadmap

## Contributing
