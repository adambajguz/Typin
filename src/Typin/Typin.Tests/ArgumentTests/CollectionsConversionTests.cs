namespace Typin.Tests.ArgumentTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomTypes.Initializable;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class CollectionsConversionTests
    {
        private readonly ITestOutputHelper _output;

        public CollectionsConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Property_of_type_object_array_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--obj-array", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                ObjectArray = new object[] { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_string_array_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-array", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringArray = new[] { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_string_IEnumerable_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-enumerable", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringEnumerable = new[] { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_string_IReadOnlyList_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-read-only-list", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringReadOnlyList = new[] { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_string_List_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-list", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringList = new List<string> { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_string_HashSet_is_bound_directly_from_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-set", "foo", "bar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringHashSet = new HashSet<string> { "foo", "bar" }
            });
        }

        [Fact]
        public async Task Property_of_type_int_array_is_bound_by_parsing_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int-array", "3", "15"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                IntArray = new[] { 3, 15 }
            });
        }

        [Fact]
        public async Task Property_of_an_enum_array_type_is_bound_by_parsing_the_argument_values_as_names()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-array", "value1", "value3"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumArray = new[] { CustomEnum.Value1, CustomEnum.Value3 }
            });
        }

        [Fact]
        public async Task Property_of_an_enum_array_type_is_bound_by_parsing_the_argument_values_as_ids()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-array", "1", "3"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumArray = new[] { CustomEnum.Value1, CustomEnum.Value3 }
            });
        }

        [Fact]
        public async Task Property_of_an_enum_array_type_is_bound_by_parsing_the_argument_values_as_either_names_or_ids()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-array", "1", "value3"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumArray = new[] { CustomEnum.Value1, CustomEnum.Value3 }
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
        public async Task Property_must_have_a_type_that_implements_IEnumerable_in_order_to_be_bound_from_multiple_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int", "1", "2", "3"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}