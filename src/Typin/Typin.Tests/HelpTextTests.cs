namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class HelpTextTests
    {
        private readonly ITestOutputHelper _output;

        public HelpTextTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Help_text_shows_usage_format_which_lists_all_parameters()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Usage",
                "cmd", "<parama>", "<paramb>", "<paramc...>"
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Help_text_shows_usage_format_which_lists_all_required_options()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithRequiredOptionsCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Usage",
                "cmd", "--opt-a <value>", "--opt-c <values...>", "[options]",
                "Options",
                "* -a|--opt-a",
                "-b|--opt-b",
                "* -c|--opt-c"
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Help_text_shows_all_valid_values_for_enum_arguments()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithEnumArgumentsCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Parameters",
                "enum", "Valid values: \"Value1\", \"Value2\", \"Value3\".",
                "Options",
                "--enum", "Valid values: \"Value1\", \"Value2\", \"Value3\".",
                "* --required-enum", "Valid values: \"Value1\", \"Value2\", \"Value3\"."
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Help_text_shows_environment_variable_names_for_options_that_have_them_defined()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithEnvironmentVariablesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Options",
                "-a|--opt-a", "Environment variable:", "ENV_OPT_A",
                "-b|--opt-b", "Environment variable:", "ENV_OPT_B"
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Help_text_shows_default_values_for_non_required_options()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithDefaultValuesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Options",
                "--obj", "Default: \"42\"",
                "--str", "Default: \"foo\"",
                "--str-empty", "Default: \"\"",
                "--str-array", "Default: \"foo\" \"bar\" \"baz\"",
                "--bool", "Default: \"True\"",
                "--char", "Default: \"t\"",
                "--int", "Default: \"1337\"",
                "--int-nullable", "Default: \"1337\"",
                "--int-array", "Default: \"1\" \"2\" \"3\"",
                "--timespan", "Default: \"02:03:00\"",
                "--enum", "Default: \"Value2\""
            );

            _output.WriteLine(stdOut.GetString());
        }

        [Fact]
        public async Task Help_text_shows_default_values_and_names_for_non_required_options()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithDefaultValuesAndNamesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "Options",
                "--object", "Default: \"42\"",
                "--string", "Default: \"foo\"",
                "--string-empty", "Default: \"\"",
                "--string-array", "Default: \"foo\" \"bar\" \"baz\"",
                "--bool", "Default: \"True\"",
                "--char", "Default: \"t\"",
                "--int", "Default: \"1337\"",
                "--int-nullable", "Default: \"1337\"",
                "--int-array", "Default: \"1\" \"2\" \"3\"",
                "--time-span", "Default: \"02:03:00\"",
                "--enum", "Default: \"Value2\""
            );

            _output.WriteLine(stdOut.GetString());
        }
    }
}