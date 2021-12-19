namespace Typin.Benchmarks.MultiCommand
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using Microsoft.Extensions.Hosting;
    using Typin.Benchmarks.MultiCommand.CliFxCommands;
    using Typin.Benchmarks.MultiCommand.TypinCommands;
    using Typin.Hosting;
    using Typin.Modes;

    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    //[RPlotExporter]
    //[MemoryDiagnoser]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
    public class Benchmarks
    {
        private static readonly string[] Arguments = { "--str", "hello world", "-i", "13", "-b" };

        #region Typin
        [Benchmark(Description = "Typin - 1 command", Baseline = true)]
        public async ValueTask ExecuteWithTypinDefaultCommandOnly()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.Single<TypinCommand>();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();
        }

        [Benchmark(Description = "Typin - 2 commands")]
        public async ValueTask ExecuteWithTypin2Commands()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.Single<TypinCommand>()
                                   .Single<TypinNamedCommand>();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();
        }

        [Benchmark(Description = "Typin - 5 commands")]
        public async ValueTask ExecuteWithTypin5Commands()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.Single<TypinCommand>()
                                   .Single<TypinNamedCommand>()
                                   .Single<TypinNamedCommand00>()
                                   .Single<TypinNamedCommand01>()
                                   .Single<TypinNamedCommand02>();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();
        }

        [Benchmark(Description = "Typin - 10 commands")]
        public async ValueTask ExecuteWithTypin10Commands()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.Single<TypinCommand>()
                                   .Single<TypinNamedCommand>()
                                   .Single<TypinNamedCommand00>()
                                   .Single<TypinNamedCommand01>()
                                   .Single<TypinNamedCommand02>()
                                   .Single<TypinNamedCommand03>()
                                   .Single<TypinNamedCommand04>()
                                   .Single<TypinNamedCommand05>()
                                   .Single<TypinNamedCommand06>()
                                   .Single<TypinNamedCommand07>();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();
        }

        [Benchmark(Description = "Typin - 20 commands")]
        public async ValueTask ExecuteWithTypin20Commands()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.FromThisAssembly();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();
        }
        #endregion

        #region CliFx
        [Benchmark(Description = "CliFx - 1 command")]
        public async ValueTask<int> ExecuteWithCliFxDefaultCommandOnly()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxCommands.CliFxCommands>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 2 commands")]
        public async ValueTask<int> ExecuteWithCliFx2Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxCommands.CliFxCommands>()
                                                          .AddCommand<CliFxNamedCommand>()
                                                          .Build()
                                                          .RunAsync(Arguments, new Dictionary<string, string>());
        }

        [Benchmark(Description = "CliFx - 5 commands")]
        public async ValueTask<int> ExecuteWithCliFx5Commands()
        {
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxCommands.CliFxCommands>()
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
            return await new CliFx.CliApplicationBuilder().AddCommand<CliFxCommands.CliFxCommands>()
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

        public static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}