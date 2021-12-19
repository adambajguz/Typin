namespace Typin.Tests.InteractiveModeTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Modes;
    using Typin.Modes.Interactive;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Valid.Commands;
    using Typin.Tests.Data.Valid.DefaultCommands;
    using Xunit;
    using Xunit.Abstractions;

    public class InteractiveModeTests
    {
        private const int Timeout = 5000;
        private readonly ITestOutputHelper _output;

        public InteractiveModeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory(Timeout = Timeout)]
        [InlineData("exit\r")]
        [InlineData("exit\n")]
        [InlineData("exit\r\n")]
        [InlineData("exit\n\r")]
        public async Task Application_should_start_interactive_mode_with_command_and_exit(string input)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().StartWith("dotnet testhost.dll> ");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory(Timeout = Timeout)]
        [InlineData("exit\r")]
        [InlineData("exit\n")]
        [InlineData("exit\r\n")]
        [InlineData("exit\n\r")]
        public async Task Application_should_start_interactive_mode_with_directive_show_help_and_exit(string input)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "[interactive] --help",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", "COMMANDS", "DIRECTIVES");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_interactive_mode_only_command()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "named-interactive-only" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", NamedInteractiveOnlyCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_not_execute_unknown_command()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "unknown-command-only" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ");
            stdErr.GetString().Should().Contain("Unrecognized parameters provided: unknown-command-only");
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_default_command()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "[!]" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", DefaultCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_scoped_command_with_root_uncope()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "[>] named-interactive-only", "[!]", "[..]" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", NamedInteractiveOnlyCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_scoped_command_and_stay_there()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "[>] named-interactive-only", "[>]", "[>] unknown command", "[>] named-interactive-only", "[!]", "[..]" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", NamedInteractiveOnlyCommand.ExpectedOutputText);
            stdErr.GetString().Should().NotContain("Unrecognized parameters provided");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_scoped_command_and_stay_there_without_executing()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "[>] named-interactive-only", "[>]", "[>] unknown command", "[>] named-interactive-only", "[..]" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain("dotnet testhost.dll> ");
            stdOut.GetString().Should().NotContain(NamedInteractiveOnlyCommand.ExpectedOutputText);
            stdErr.GetString().Should().NotContain("Unrecognized parameters provided");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact(Timeout = Timeout)]
        public async Task Application_should_execute_scoped_command_with_up_unscope()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddCommand<ExitCommand>()
                .RegisterMode<DirectMode>(asStartup: true)
                .RegisterMode<InteractiveMode>();

            string input = new[] { "[>] named-interactive-only", "[!]", "[.]" }.JoinToInteractiveCommand();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output,
                                                                                commandLine: "interactive",
                                                                                isInputRedirected: true,
                                                                                input: input);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("dotnet testhost.dll> ", NamedInteractiveOnlyCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}