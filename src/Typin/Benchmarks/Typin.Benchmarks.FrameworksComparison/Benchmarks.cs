﻿namespace Typin.Benchmarks.FrameworksComparison
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using CommandLine;
    using Typin.Benchmarks.FrameworksComparison.Commands;

    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public class Benchmarks
    {
        private static readonly string[] Arguments = { "--str", "hello world", "-i", "13", "-b" };

        [Benchmark(Description = "TypinWarm")]
        public async ValueTask<int> ExecuteWithTypinWarmDefaultCommandOnly()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "Typin", Baseline = true)]
        public async ValueTask<int> ExecuteWithTypinDefaultCommandOnly()
        {
            return await new CliApplicationBuilder().AddCommand<TypinCommand>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx")]
        public async ValueTask<int> ExecuteWithCliFx()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxCommand>()
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
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}