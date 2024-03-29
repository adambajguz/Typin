﻿namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Directives;
    using Typin.Modes;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class HelpWriterTests
    {
        private readonly ITestOutputHelper _output;

        public HelpWriterTests(ITestOutputHelper output)
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
                "USAGE",
                "PARAMETERS",
                "cmd", "<param-a>", "<param-b>", "<param-c...>"
            );
        }

        [Fact]
        public async Task Help_text_shows_directives()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .AddDirective<DebugDirective>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "USAGE",
                "PARAMETERS",
                "DIRECTIVES",
                "[debug]",
                "cmd", "<param-a>", "<param-b>", "<param-c...>"
            );
        }

        [Fact]
        public async Task Help_text_shows_modes()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddDirective<DebugDirective>()
                .UseDirectMode(true)
                .UseInteractiveMode();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "USAGE",
                "DIRECTIVES",
                "SUPPORTED MODES",
                typeof(DirectMode).FullName,
                typeof(Modes.InteractiveMode).FullName,
                "@ interactive",
                "@ named-interactive-only",
                "@ [.]",
                "@ [..]",
                "@ [>]",
                "@ [interactive]",
                "[debug]",
                "[!]"
            );
        }

        [Fact]
        public async Task Help_text_should_not_show_interactive_mode_addons_when_switched_off()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .AddCommand<NamedInteractiveOnlyCommand>()
                .AddDirective<DebugDirective>()
                .UseDirectMode(true)
                .UseInteractiveMode(
                    false,
                    (options) =>
                    {

                    },
                    new InteractiveModeBuilderSettings
                    {
                        AddInteractiveCommand = false,
                        AddInteractiveDirective = false,
                        AddScopeDirectives = false,
                    });

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "USAGE",
                "DIRECTIVES",
                "SUPPORTED MODES",
                typeof(DirectMode).FullName,
                typeof(Modes.InteractiveMode).FullName,
                "@ named-interactive-only",
                "[debug]",
                "[!]"
            );
            stdOut.GetString().Should().NotContainAll(
                "@ interactive",
                "@ [.]",
                "@ [..]",
                "@ [>]",
                "@ [interactive]"
            );
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
                "USAGE",
                "cmd", "--opt-a <value>", "--opt-c <values...>", "[options]",
                "OPTIONS",
                "* -a|--opt-a",
                "-b|--opt-b",
                "* -c|--opt-c"
            );
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
                "PARAMETERS",
                "enum",
                "(One of: \"Value1\", \"Value2\", \"Value3\")",
                "OPTIONS",
                "--enum",
                "--arr-enum",
                "(Array of: \"Value1\", \"Value2\", \"Value3\", \"\")",
                "* --required-enum"
            );
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
                "OPTIONS",
                "-a|--opt-a", "Fallback variable:", "ENV_OPT_A",
                "-b|--opt-b", "Fallback variable:", "ENV_OPT_B"
            );
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
                "OPTIONS",
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
                "OPTIONS",
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
        }

        [Fact]
        public async Task Help_text_shows_all_valid_values_for_non_scalar_enum_parameters_and_options()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithEnumCollectionArgumentsCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "PARAMETERS",
                "OPTIONS",
                "Foo",
                "bar",
                "wizz",
                "Buzz",
                "fizzz",
                "Value1", "Value2", "Value3",
                "Value4", "Value5", "Value6",
                "Value7", "Value8", "Value9",
                "ValueA", "ValueB", "ValueC",
                @"(Default: ""ValueD"" ""ValueF"")"
            );
        }

        [Fact]
        public async Task Help_text_shows_all_valid_values_for_non_scalar_nullable_enum_parameters_and_options()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithNullableEnumCollectionArgumentsCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll(
                "PARAMETERS",
                "OPTIONS",
                "Foo",
                "bar",
                "wizz",
                "Buzz",
                "fizzz",
                @"""""",
                "Value1", "Value2", "Value3",
                "Value4", "Value5", "Value6",
                "Value7", "Value8", "Value9",
                "ValueA", "ValueB", "ValueC",
                @"(Default: ""ValueD"" ""ValueF"")"
            );
        }
    }
}