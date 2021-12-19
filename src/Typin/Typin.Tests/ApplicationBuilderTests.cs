namespace Typin.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Modes;
    using Typin.Modes.Interactive;
    using Typin.Tests.Data.Valid.Commands;
    using Typin.Tests.Data.Valid.CustomDirectives;
    using Typin.Tests.Data.Valid.Modes;
    using Xunit;
    using Xunit.Abstractions;
    using Typin.Tests.Data.Common.Extensions;

    public class ApplicationBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public ApplicationBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Application_can_be_created_with_a_default_configuration_and_multiple_commands()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_can_be_created_with_a_default_configuration_and_one_command()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_in_direct_mode_can_be_created_with_a_custom_configuration()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>()
                .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                //.AddDynamicCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                //.UseOptionFallbackProvider<EnvironmentVariableFallbackProvider>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage($"Startup message")
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_in_direct_mode_can_be_created_with_a_custom_configuration_and_middlewares()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>()
                .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                //.AddDynamicCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective(typeof(CustomDirective))
                //.UseMiddleware<ExecutionTimingMiddleware>()
                //.UseMiddleware(typeof(ExitCodeMiddleware))
                //.UseOptionFallbackProvider(typeof(EnvironmentVariableFallbackProvider))
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_in_interactive_mode_can_be_created_with_a_custom_configuration()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>()
                .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                //.AddDynamicCommandsFromThisAssembly()
                .AddDirectivesFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>()
                .RegisterMode<ValidCustomMode>()
                .RegisterMode(typeof(ValidCustomMode))
                .UseStartupMessage((metadata, console) => { console.Output.WriteLine($"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description})"); })
                .UseConsole(new VirtualConsole(Stream.Null));

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_in_interactive_mode_with_params_can_be_created_with_a_custom_configuration()
        {
            // Act
            var builder = new CliApplicationBuilder()
               .AddCommand<BenchmarkDefaultCommand>()
               .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
               .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
               .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
               .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
               //.AddDynamicCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .AddDirective<PreviewDirective>()
               .AddDirectivesFromThisAssembly()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .RegisterMode<DirectMode>(asStartup: true)
               .RegisterMode<InteractiveMode>()
               .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
               .UseConsole<SystemConsole>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_can_be_created_with_VirtualConsole_MemoryStreamWriter()
        {
            // Arrange
            var (console, _, _) = VirtualConsole.CreateBuffered(isInputRedirected: false, isOutputRedirected: true);

            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>()
                .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                .AddDirectivesFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseConsole(console)
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_can_be_created_with_VirtualConsole_CreateBuffered()
        {
            // Arrange
            var (console, _, _) = VirtualConsole.CreateBuffered(isInputRedirected: false);

            // Act
            var builder = new CliApplicationBuilder()
                .AddCommand<BenchmarkDefaultCommand>()
                .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(console)
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_can_be_created_with_configuration_action()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .Configure(cfg =>
                {
                    cfg.AddCommand<BenchmarkDefaultCommand>()
                       .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
                       .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
                       .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                       .AddCommandsFrom(typeof(AddValidDynamicAndExecuteCommand).Assembly)
                       .RegisterMode<DirectMode>(asStartup: true)
                       .RegisterMode<InteractiveMode>()
                       .AddDirective<DebugDirective>()
                       .AddDirective<PreviewDirective>()
                       .AddDirective<CustomInteractiveModeOnlyDirective>()
                       .AddDirective<CustomDirective>();

                    cfg.UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}");
                })
                .Configure(cfg =>
                {
                    cfg.UseTitle("test")
                       .UseExecutableName("test")
                       .UseVersionText("test")
                       .UseDescription("test")
                       .UseConsole<SystemConsole>();
                });

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }
    }
}