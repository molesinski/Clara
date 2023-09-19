using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace Clara.Benchmarks
{
    public class Program
    {
        private static void Main()
        {
            var config = ManualConfig.CreateEmpty()
                .AddColumnProvider(DefaultColumnProviders.Instance)
                .AddLogger(ConsoleLogger.Default);

            BenchmarkRunner.Run<IndexBenchmarks>(config);
        }
    }
}
