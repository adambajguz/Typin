namespace Typin.Tests.ArgumentTests
{
    using System;
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
        [InlineData(typeof(InvalidParameterConverterCommand))]
        [InlineData(typeof(InvalidOptionConverterCommand))]
        public async Task Application_should_not_run_with_invalid_argument_converter(Type commandType)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand(commandType);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new string[] { "cmd" }, isInputRedirected: false);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain($"Command argument has an invalid converter");
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
            stdOut.GetString().Should().ContainAll(
                @"{""Value"":1235,""Day"":6}",
                @""":null");
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
        public async Task Property_of_custom_enumerable_type_can_be_initialized_via_binding_converter_Convert(string optionName)
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
            stdOut.GetString().Should().ContainAll(
                @"{""Value"":0,""Day"":0}",
                @""":null",
                @"[""Monday"",""1235""]");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("--str-enumerable")]
        [InlineData("--str-indirect-enumerable")]
        public async Task Property_of_custom_enumerable_type_can_be_initialized_via_binding_converter_ConvertCollection(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName, "Monday:1235", "Friday:7890", "0875"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().StartWith(@"{""StringInitializable"":");
            stdOut.GetString().Should().ContainAll(
                @"{""Value"":0,""Day"":0}",
                @""":null",
                @"""Monday"",""1235"",""Friday"",""7890"",""0875""");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Property_of_custom_type_that_is_nullable_struct_can_be_initialized_even_if_failing_binding_converter_was_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaFailingConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-nullable-struct-by-non-nullable-converter"
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().DeserializeJson<SupportedArgumentTypesViaConverterCommand>().Should().BeEquivalentTo(new SupportedArgumentTypesViaConverterCommand());
            stdOut.GetString().Should().StartWith(@"{""StringInitializable"":null,""StringNullableInitializableStruct"":null,""StringNullableInitializableStructByNonNullableConverter"":null,""StringInitializableStruct"":{""Value"":0,""Day"":0},""StringEnumerableInitializable"":null,""IndirectlyStringEnumerableInitializable"":null}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        #region Failing
        [Theory]
        [InlineData("--str-class")]
        [InlineData("--str-nullable-struct")]
        [InlineData("--str-nullable-struct-by-non-nullable-converter")]
        [InlineData("--str-struct")]
        public async Task Property_of_custom_type_cannot_be_initialized_via_failing_binding_converter(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaFailingConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName, "Saturday:1235"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().NotStartWith(@"{""StringInitializable"":");
            stdOut.GetString().Should().NotContainAll(@"{""Value"":1235,""Day"":6}", @""":null");
            stdErr.GetString().Should().ContainAll("The method or operation is not implemented.", "Can't convert value \"", optionName);
        }

        [Theory]
        [InlineData("--str-class")]
        [InlineData("--str-nullable-struct")]
        [InlineData("--str-struct")]
        public async Task Property_of_custom_type_cannot_be_initialized_via_failing_binding_converter_with_respect_to_nullable_types(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaFailingConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().DeserializeJson<SupportedArgumentTypesViaConverterCommand>().Should().NotBeEquivalentTo(new SupportedArgumentTypesViaConverterCommand());
            stdOut.GetString().Should().NotStartWith(@"{""StringInitializable"":null,""StringNullableInitializableStruct"":null,""StringNullableInitializableStructByNonNullableConverter"":null,""StringInitializableStruct"":{""Value"":0,""Day"":0},""StringEnumerableInitializable"":null,""IndirectlyStringEnumerableInitializable"":null}");
            stdErr.GetString().Should().ContainAll("The method or operation is not implemented.", "Can't convert value \"", optionName);
        }

        [Theory]
        [InlineData("--str-enumerable")]
        [InlineData("--str-indirect-enumerable")]
        public async Task Property_of_custom_enumerable_type_cannot_be_initialized_via_failing_binding_converter(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesViaFailingConverterCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", optionName, "Monday:1235"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().NotStartWith(@"{""StringInitializable"":");
            stdOut.GetString().Should().NotContainAll(@"{""Value"":1235,""Day"":6}", @""":null");
            stdErr.GetString().Should().Contain("The method or operation is not implemented.");
            stdErr.GetString().Should().ContainAll("The method or operation is not implemented.", "Can't convert values [\"", optionName);
        }
        #endregion
    }
}