namespace Typin.Benchmarks.MultiCommand
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using Typin.Benchmarks.MultiCommand.CliFxComands;
    using Typin.Benchmarks.MultiCommand.TypinCommands;

    //[SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    //[SimpleJob(RuntimeMoniker.CoreRt31)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    //[RPlotExporter]
    //[MemoryDiagnoser]
    public class Benchmarks
    {
        private static readonly string[] Arguments = { "--str", "hello world", "-i", "13", "-b" };

#pragma warning disable CA1822 // Mark members as static
        #region Typin
        [Benchmark(Description = "Typin - warmup")]
        public async ValueTask<int> ExecuteWithTypinWarmup()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommands.TypinCommands>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 1 command", Baseline = true)]
        public async ValueTask<int> ExecuteWithTypinDefaultCommandOnly()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommands.TypinCommands>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 2 commands")]
        public async ValueTask<int> ExecuteWithTypin2Commands()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommands.TypinCommands>()
                                                    .AddCommand<TypinNamedCommand>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 5 commands")]
        public async ValueTask<int> ExecuteWithTypin5Commands()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommands.TypinCommands>()
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
            return await new CliApplicationBuilder().AddCommand<TypinCommands.TypinCommands>()
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
        #endregion

        #region CliFx
        [Benchmark(Description = "CliFx - 1 command")]
        public async ValueTask<int> ExecuteWithCliFxDefaultCommandOnly()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxComands.CliFxCommands>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 2 commands")]
        public async ValueTask<int> ExecuteWithCliFx2Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxComands.CliFxCommands>()
                                                          .AddCommand<CliFxNamedCommand>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 5 commands")]
        public async ValueTask<int> ExecuteWithCliFx5Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxComands.CliFxCommands>()
                                                          .AddCommand<CliFxNamedCommand>()
                                                          .AddCommand<CliFxNamedCommand00>()
                                                          .AddCommand<CliFxNamedCommand01>()
                                                          .AddCommand<CliFxNamedCommand02>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 10 commands")]
        public async ValueTask<int> ExecuteWithCliFx10Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxComands.CliFxCommands>()
                                                          .AddCommand<CliFxNamedCommand>()
                                                          .AddCommand<CliFxNamedCommand00>()
                                                          .AddCommand<CliFxNamedCommand01>()
                                                          .AddCommand<CliFxNamedCommand02>()
                                                          .AddCommand<CliFxNamedCommand03>()
                                                          .AddCommand<CliFxNamedCommand04>()
                                                          .AddCommand<CliFxNamedCommand05>()
                                                          .AddCommand<CliFxNamedCommand06>()
                                                          .AddCommand<CliFxNamedCommand07>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 20 commands")]
        public async ValueTask<int> ExecuteWithCliFx22Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }
        #endregion
#pragma warning restore CA1822 // Mark members as static

        public static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}