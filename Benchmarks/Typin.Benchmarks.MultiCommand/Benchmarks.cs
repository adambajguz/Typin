namespace Typin.Benchmarks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using Typin.Benchmarks.MultiCommand.Commands;

    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class Benchmarks
    {
        private static readonly string[] Arguments = { "--str", "hello world", "-i", "13", "-b" };

        [Benchmark(Description = "Typin - 1 command", Baseline = true)]
        public async ValueTask<int> ExecuteWithTypinDefaultCommandOnly()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 2 commands")]
        public async ValueTask<int> ExecuteWithTypin2Commands()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .AddCommand<TypinNamedCommand>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 5 commands")]
        public async ValueTask<int> ExecuteWithTypin5Commands()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .AddCommand<TypinNamedCommand>()
                                                    .AddCommand<TypinNamedCommand00>()
                                                    .AddCommand<TypinNamedCommand01>()
                                                    .AddCommand<TypinNamedCommand02>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 10 commands")]
        public async ValueTask<int> ExecuteWithTypin10Commands()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .AddCommand<TypinNamedCommand>()
                                                    .AddCommand<TypinNamedCommand00>()
                                                    .AddCommand<TypinNamedCommand01>()
                                                    .AddCommand<TypinNamedCommand02>()
                                                    .AddCommand<TypinNamedCommand03>()
                                                    .AddCommand<TypinNamedCommand04>()
                                                    .AddCommand<TypinNamedCommand05>()
                                                    .AddCommand<TypinNamedCommand06>()
                                                    .AddCommand<TypinNamedCommand07>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 20 commands")]
        public async ValueTask<int> ExecuteWithTypin22Commands()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        public static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}