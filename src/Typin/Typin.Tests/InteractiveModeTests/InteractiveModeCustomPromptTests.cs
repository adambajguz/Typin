//namespace Typin.Tests.InteractiveMode
//{
//    using System;
//    using System.Threading.Tasks;
//    using FluentAssertions;
//    using Typin.Modes;
//    using Typin.Modes.Interactive;
//    using Typin.Tests.Data.Commands.Valid;
//    using Typin.Tests.Extensions;
//    using Xunit;
//    using Xunit.Abstractions;

namespace Typin.Tests.InteractiveModeTests
{
    //    public class InteractiveModeCustomPromptTests
    //    {
    //        private const int Timeout = 5000;
    //        private readonly ITestOutputHelper _output;

    //        public InteractiveModeCustomPromptTests(ITestOutputHelper output)
    //        {
    //            _output = output;
    //        }


    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_string_based_prompt()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseDirectMode(asStartup: true)
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.PromptForeground = ConsoleColor.Magenta;
    //                    cfg.SetPrompt("~$ ");
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith("~$ ");
    //            stdErr.GetString().Should().BeNullOrWhiteSpace();
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_have_simple_string_prompt_with_scope()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt("~$ ");
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: "[>] exit\r[..]\r");

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith("~$ ");
    //            stdOut.GetString().Should().Contain("exit ~$ ");
    //            stdErr.GetString().Should().BeNullOrWhiteSpace();
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_multiple_calls_of_set_prompt()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt("~$ ");
    //                    cfg.SetDefaultPrompt();
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith("dotnet testhost.dll> ");
    //            stdErr.GetString().Should().BeNullOrWhiteSpace();
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_func_based_prompt_with_metadata()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseTitle("testTitle")
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt((metadata) => metadata.Title);
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith("testTitle");
    //            stdErr.GetString().Should().BeNullOrWhiteSpace();
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_action_based_prompt_with_metadata_and_console()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseTitle("testTitle")
    //                .UseExecutableName("otherTitle")
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt((metadata, console) =>
    //                    {
    //                        console.Output.Write(metadata.Title);
    //                        console.Error.Write(metadata.ExecutableName);
    //                    });
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith("testTitle");
    //            stdErr.GetString().Should().StartWith("otherTitle");
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_action_based_prompt_with_options_metadata_and_console()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseTitle("testTitle")
    //                .UseExecutableName("otherTitle")
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt((interactiveModeOptions, metadata, console) =>
    //                    {
    //                        console.Output.Write(interactiveModeOptions.PromptForeground);
    //                        console.Output.Write(" ");
    //                        console.Output.Write(metadata.Title);
    //                        console.Error.Write(metadata.ExecutableName);
    //                    });
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith($"{ConsoleColor.Blue} testTitle");
    //            stdErr.GetString().Should().StartWith("otherTitle");
    //        }

    //        [Fact(Timeout = Timeout)]
    //        public async Task Application_should_allow_action_based_prompt_with_service_options_metadata_and_console()
    //        {
    //            // Arrange
    //            var builder = new CliApplicationBuilder()
    //                .AddCommand<DefaultCommand>()
    //                .AddCommand<ExitCommand>()
    //                .RegisterMode<DirectMode>(asStartup: true)
    //                .RegisterMode<InteractiveMode>()
    //                .UseTitle("testTitle")
    //                .UseExecutableName("otherTitle")
    //                .UseInteractiveMode(options: (cfg) =>
    //                {
    //                    cfg.SetPrompt<ICliApplicationLifetime>((lifetime, interactiveModeOptions, metadata, console) =>
    //                       {
    //                           console.Output.Write(lifetime.CurrentModeType?.Name);
    //                           console.Output.Write(interactiveModeOptions.PromptForeground);
    //                           console.Output.Write(" ");
    //                           console.Output.Write(metadata.Title);
    //                           console.Error.Write(metadata.ExecutableName);
    //                       });
    //                });

    //            // Act
    //            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
    //                                                                                commandLine: "interactive",
    //                                                                                isInputRedirected: true,
    //                                                                                input: string.Empty);

    //            // Assert
    //            exitCode.Should().Be(ExitCode.Success);
    //            stdOut.GetString().Should().StartWith($"InteractiveMode{ConsoleColor.Blue} testTitle");
    //            stdErr.GetString().Should().StartWith("otherTitle");
    //        }
    //    }
    //}
}