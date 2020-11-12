namespace Typin.Tests.ArgumentTests
{
    using System.Globalization;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomTypes.Initializable;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ParsableArgumentConversionTests
    {
        private readonly ITestOutputHelper _output;

        public ParsableArgumentConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Property_of_a_type_that_has_a_constructor_accepting_a_string_is_bound_by_invoking_the_constructor_with_the_argument_value()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-constructible", "foobar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringConstructible = new CustomStringConstructible("foobar")
            });
        }

        [Fact]
        public async Task Property_of_an_array_of_type_that_has_a_constructor_accepting_a_string_is_bound_by_invoking_the_constructor_with_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-constructible-array", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringConstructibleArray = new[]
                {
                    new CustomStringConstructible("foo"),
                    new CustomStringConstructible("bar")
                }
            });
        }

        [Fact]
        public async Task Property_of_a_type_that_has_a_static_Parse_method_accepting_a_string_is_bound_by_invoking_the_method()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-parseable", "foobar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringParseable = CustomStringParseable.Parse("foobar")
            });
        }

        [Fact]
        public async Task Property_of_a_type_that_has_a_static_Parse_method_accepting_a_string_and_format_provider_is_bound_by_invoking_the_method()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-parseable-format", "foobar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringParseableWithFormatProvider = CustomStringParseableWithFormatProvider.Parse("foobar", CultureInfo.InvariantCulture)
            });
        }
    }
}