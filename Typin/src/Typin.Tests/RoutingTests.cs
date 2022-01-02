namespace Typin.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Valid.Commands;
    using Typin.Tests.Data.Valid.DefaultCommands;
    using Xunit;
    using Xunit.Abstractions;

    public class RoutingTests
    {
        private readonly ITestOutputHelper _output;

        public RoutingTests(ITestOutputHelper testOutput)
        {
            _output = testOutput;
        }

        [Fact]
        public async Task Default_command_is_executed_if_provided_arguments_do_not_match_any_named_command()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, Array.Empty<string>());

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Trim().Should().Be(DefaultCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Specific_named_command_is_executed_if_provided_arguments_match_its_name()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "named" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Trim().Should().Be(NamedCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Specific_named_sub_command_is_executed_if_provided_arguments_match_its_name()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "named", "sub" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Trim().Should().Be(NamedSubCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Help_text_is_printed_if_no_arguments_were_provided_and_default_command_is_not_defined()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>()
                .UseDescription("This will be visible in help");

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, Array.Empty<string>());

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain("This will be visible in help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Help_text_is_printed_if_provided_arguments_contain_the_help_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll("Default command description", "Usage".ToUpperInvariant());
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Help_text_is_printed_if_provided_arguments_contain_the_help_option_even_if_default_command_is_not_defined()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>()
                .UseDescription("This will be visible in help");

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain("This will be visible in help");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Help_text_for_a_specific_named_command_is_printed_if_provided_arguments_match_its_name_and_contain_the_help_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "named", "--help" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll(
                "Named command description",
                "Usage".ToUpperInvariant(),
                "named"
            );
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Help_text_for_a_specific_named_sub_command_is_printed_if_provided_arguments_match_its_name_and_contain_the_help_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "named", "sub", "--help" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().ContainAll(
                "Named sub command description",
                "Usage".ToUpperInvariant(),
                "named", "sub"
            );
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Version_is_printed_if_the_only_provided_argument_is_the_version_option()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommand>()
                .AddCommand<NamedCommand>()
                .AddCommand<NamedSubCommand>()
                .UseVersionText("v6.9");

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "--version" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Trim().Should().Be("v6.9");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}