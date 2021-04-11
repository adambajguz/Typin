namespace Typin.Tests.ArgumentTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ArgumentBindingConverterTests
    {
        private readonly ITestOutputHelper _output;

        public ArgumentBindingConverterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("--str-class")]
        [InlineData("--str-nullable-struct")]
        [InlineData("--str-nullable-struct-by-non-nullable-converter")]
        [InlineData("--str-struct")]
        public async Task Property_of_custom_type_can_be_initialized_via_binding_converter(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName, "Saturday:1235"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().StartWith(@"{""StringInitializable"":");
            stdOut.GetString().Should().ContainAll(@"{""Value"":1235,""Day"":6}", @""":null");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("--str-class")]
        [InlineData("--str-nullable-struct")]
        [InlineData("--str-nullable-struct-by-non-nullable-converter")]
        [InlineData("--str-struct")]
        public async Task Property_of_custom_type_can_be_initialized_via_binding_converter_with_respect_to_nullable_types(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().DeserializeJson<SupportedArgumentTypesViaConverterCommand>().Should().BeEquivalentTo(new SupportedArgumentTypesViaConverterCommand());
            stdOut.GetString().Should().StartWith(@"{""StringInitializable"":null,""StringNullableInitializableStruct"":null,""StringNullableInitializableStructByNonNullableConverter"":null,""StringInitializableStruct"":{""Value"":0,""Day"":0},""StringEnumerableInitializable"":null,""IndirectlyStringEnumerableInitializable"":null}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("--str-enumerable")]
        [InlineData("--str-indirect-enumerable")]
        public async Task Property_of_custom_enumerable_type_can_be_initialized_via_binding_converter(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName, "Monday:1235"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().StartWith(@"{""StringInitializable"":");
            stdOut.GetString().Should().ContainAll(@"{""Value"":1235,""Day"":6}", @""":null");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}