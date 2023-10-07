using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace Clara.Benchmarks
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var config = ManualConfig.CreateEmpty()
                .AddColumnProvider(DefaultColumnProviders.Instance)
                .AddLogger(ConsoleLogger.Default)
                .AddExporter(MarkdownExporter.GitHub);

            var switcher = new BenchmarkSwitcher(
                new[]
                {
                    typeof(TokenizationBenchmarks),
                    typeof(IndexingBenchmarks),
                    typeof(QueryingBenchmarks),
                });

            switcher.Run(args, config);
        }
    }
}
