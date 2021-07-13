namespace Typin.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Exceptions;
    using Typin.Modes;
    using Typin.OptionFallback;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Valid;
    using Typin.Tests.Data.Middlewares;
    using Typin.Tests.Data.Modes.Valid;
    using Typin.Tests.Data.Startups;
    using Typin.Tests.Data.Valid.Extensions;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

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
                .AddCommandsFromValidAssembly()
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public void Application_can_be_build_only_once()
        {
            // Arrange
            var builder = new CliApplicationBuilder().AddCommandsFromThisAssembly();

            // Act
            Action act = () =>
            {
                builder.Build();
                builder.Build();
            };

            // Assert
            act.Should().Throw<InvalidOperationException>();
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
            exitCode.Should().Be(ExitCodes.Success);
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
                .AddCommandsFromValidAssembly()
                .AddDynamicCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomDirective>()
                .UseOptionFallbackProvider<EnvironmentVariableFallbackProvider>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage($"Startup message")
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
                .AddCommandsFromValidAssembly()
                .AddDynamicCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective(typeof(CustomDirective))
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseMiddleware(typeof(ExitCodeMiddleware))
                .UseOptionFallbackProvider(typeof(EnvironmentVariableFallbackProvider))
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(new VirtualConsole(Stream.Null))
                .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
                .AddCommandsFromValidAssembly()
                .AddDynamicCommandsFromThisAssembly()
                .AddDirectivesFrom(typeof(BenchmarkDefaultCommand).Assembly)
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseDirectMode(true)
                .UseInteractiveMode()
                .RegisterMode<ValidCustomMode>()
                .RegisterMode(typeof(ValidCustomMode))
                .UseInteractiveMode()
                .UseStartupMessage((metadata, console) => { console.Output.WriteLine($"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description})"); })
                .UseConsole(new VirtualConsole(Stream.Null));

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
               .AddCommandsFromValidAssembly()
               .AddDynamicCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .AddDirective<PreviewDirective>()
               .AddDirectivesFromThisAssembly()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .UseDirectMode(true)
               .UseInteractiveMode()
               .UseStartupMessage((metadata) => $"Startup message {metadata.Title} {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}")
               .UseConsole<SystemConsole>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
                .AddCommandsFromValidAssembly()
                .AddDirectivesFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseConsole(console)
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
                .AddCommandsFromValidAssembly()
                .UseExceptionHandler(typeof(DefaultExceptionHandler))
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .AddDirective<CustomDirective>()
                .UseTitle("test")
                .UseExecutableName("test")
                .UseVersionText("test")
                .UseDescription("test")
                .UseConsole(console)
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task Application_can_be_created_with_startup_class()
        {
            // Act
            var builder = new CliApplicationBuilder()
                .UseStartup<CustomStartupClass>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "--help");

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
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
                       .AddCommandsFromValidAssembly()
                       .UseExceptionHandler<DefaultExceptionHandler>()
                       .UseDirectMode(true)
                       .UseInteractiveMode()
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
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll("USAGE", "OPTIONS");
            stdErr.GetString().Should().BeEmpty();
        }
    }
}