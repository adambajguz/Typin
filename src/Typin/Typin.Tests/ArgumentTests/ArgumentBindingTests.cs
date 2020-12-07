namespace Typin.Tests.ArgumentTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Directives;
    using Typin.Tests.Data.Commands.Invalid;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ArgumentBindingTests
    {
        private readonly ITestOutputHelper _output;

        public ArgumentBindingTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Property_annotated_as_an_option_can_be_bound_from_multiple_values_even_if_the_inputs_use_mixed_naming()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithStringArrayOptionCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--opt", "foo", "-o", "bar", "--opt", "baz"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<WithStringArrayOptionCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new WithStringArrayOptionCommand
            {
                Opt = new[] { "foo", "bar", "baz" }
            });
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithSingleRequiredOptionCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--opt-a", "foo"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_bound_to_some_value()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithSingleRequiredOptionCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--opt-a"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_bound_to_at_least_one_value_if_it_expects_multiple_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithRequiredOptionsCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--opt-a", "foo"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(13)]
        [InlineData(-13)]
        public async Task Property_annotated_as_parameter_is_bound_directly_from_argument_value_according_to_the_order(int number)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "foo", number.ToString(), "bar", "baz"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<WithParametersCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            commandInstance.Should().BeEquivalentTo(new WithParametersCommand
            {
                ParamA = "foo",
                ParamB = number,
                ParamC = new[] { "bar", "baz" }
            });
        }

        [Fact]
        public async Task Property_annotated_as_parameter_is_bound_directly_from_argument_value_according_to_the_order_even_if_there_is_a_hyphen()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "-", "0", "bar", "-", "baz"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<WithParametersCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdErr.GetString().Should().BeNullOrWhiteSpace();

            commandInstance.Should().BeEquivalentTo(new WithParametersCommand
            {
                ParamA = "-",
                ParamB = 0,
                ParamC = new[] { "bar", "-", "baz" }
            });
        }

        [Theory]
        [InlineData(typeof(NonLetterOptionName0Command))]
        [InlineData(typeof(NonLetterOptionName1Command))]
        [InlineData(typeof(NonLetterOptionName2Command))]
        [InlineData(typeof(NonLetterOptionName3Command))]
        public async Task Option_name_should_not_start_with_char_other_than_letter(Type commandType)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand(commandType);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "-h"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(typeof(NonLetterOptionShortName0Command))]
        [InlineData(typeof(NonLetterOptionShortName1Command))]
        [InlineData(typeof(NonLetterOptionShortName2Command))]
        [InlineData(typeof(NonLetterOptionShortName3Command))]
        public async Task Option_short_name_should_not_start_with_char_other_than_letter(Type commandType)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand(commandType);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "-h"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Property_annotated_as_parameter_must_always_be_bound_to_some_value()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithSingleParameterCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Property_annotated_as_parameter_must_always_be_bound_to_at_least_one_value_if_it_expects_multiple_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "foo", "13"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("cmd", "--non-existing-option", "13")]
        [InlineData("cmd", "--non-existing-option", "13", "--non2")]
        [InlineData("cmd", "--non-existing-option", "13", "non2")]
        [InlineData("cmd", "non-existing-parameter")]
        [InlineData("cmd", "--non-existing-option", "13", "non-existing-parameter")]
        [InlineData("cmd", "non-existing-parameter", "--non-existing-option", "13")]
        public async Task All_provided_parameter_and_option_arguments_must_be_bound_to_corresponding_properties(params string[] args)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Named_command_should_execute_even_when_default_takes_a_parameter()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<DefaultCommandWithParameter>()
                .AddCommand<NamedCommand>()
                .AddDirective<DefaultDirective>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[] { "named" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdOut.GetString().Should().Contain(NamedCommand.ExpectedOutputText);
        }
    }
}