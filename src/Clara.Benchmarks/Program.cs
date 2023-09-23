using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace Clara.Benchmarks
{
    public class Program
    {
        private static void Main()
        {
            var benchmarks =
                new[]
                {
                    //// typeof(IndexBenchmarks),
                    typeof(QueryBenchmarks),
                };

            var config = ManualConfig.CreateEmpty()
                .AddColumnProvider(DefaultColumnProviders.Instance)
                .AddLogger(ConsoleLogger.Default)
                .AddExporter(MarkdownExporter.GitHub);

            BenchmarkRunner.Run(benchmarks, config);
        }
    }
}
