namespace Typin.Benchmarks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using CommandLine;
    using Typin.Benchmarks.Commands;
    using Typin.Benchmarks.Commands.TypinCommands;

    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class Benchmarks
    {
        private static readonly string[] Arguments = { "--str", "hello world", "-i", "13", "-b" };

        [Benchmark(Description = "Typin - 1 command", Baseline = true)]
        public async ValueTask<int> ExecuteWithTypinDefaultCommandOnly()
        {
            return await new CliApplicationBuilder().AddCommand(typeof(CliFxCommand))
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 2 commands")]
        public async ValueTask<int> ExecuteWithTypin2Commands()
        {
            return await new CliApplicationBuilder().AddCommand(typeof(CliFxCommand))
                                                    .AddCommand(typeof(TypinNamedCommand))
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 10 commands")]
        public async ValueTask<int> ExecuteWithTypin10Commands()
        {
            return await new CliApplicationBuilder().AddCommand(typeof(CliFxCommand))
                                                    .AddCommand(typeof(TypinNamedCommand))
                                                    .AddCommand(typeof(TypinNamedCommand00))
                                                    .AddCommand(typeof(TypinNamedCommand01))
                                                    .AddCommand(typeof(TypinNamedCommand02))
                                                    .AddCommand(typeof(TypinNamedCommand03))
                                                    .AddCommand(typeof(TypinNamedCommand04))
                                                    .AddCommand(typeof(TypinNamedCommand05))
                                                    .AddCommand(typeof(TypinNamedCommand06))
                                                    .AddCommand(typeof(TypinNamedCommand07))
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin - 22 commands")]
        public async ValueTask<int> ExecuteWithTypin22Commands()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx", Baseline = true)]
        public async ValueTask<int> ExecuteWithCliFx()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand(typeof(CliFxCommand))
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "System.CommandLine")]
        public async Task<int> ExecuteWithSystemCommandLine()
        {
            return await new SystemCommandLineCommand().ExecuteAsync(Arguments);
        }

        [Benchmark(Description = "McMaster.Extensions.CommandLineUtils")]
        public int ExecuteWithMcMaster()
        {
            return McMaster.Extensions.CommandLineUtils.CommandLineApplication.Execute<McMasterCommand>(Arguments);
        }

        [Benchmark(Description = "CommandLineParser")]
        public void ExecuteWithCommandLineParser()
        {
            new Parser()
                .ParseArguments(Arguments, typeof(CommandLineParserCommand))
                .WithParsed<CommandLineParserCommand>(c => c.Execute());
        }

        [Benchmark(Description = "PowerArgs")]
        public void ExecuteWithPowerArgs()
        {
            PowerArgs.Args.InvokeMain<PowerArgsCommand>(Arguments);
        }

        [Benchmark(Description = "Clipr")]
        public void ExecuteWithClipr()
        {
            clipr.CliParser.Parse<CliprCommand>(Arguments).Execute();
        }

        [Benchmark(Description = "Cocona")]
        public void ExecuteWithCocona()
        {
            Cocona.CoconaApp.Run<CoconaCommand>(Arguments);
        }

        public static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}