namespace Typin.Tests.ArgumentTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomTypes.Initializable;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class NullableArgumentConversionTests
    {
        private readonly ITestOutputHelper _output;

        public NullableArgumentConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Property_of_type_nullable_int_is_bound_by_parsing_the_argument_value_if_it_is_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int-nullable", "15"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                IntNullable = 15
            });
        }

        [Fact]
        public async Task Property_of_type_nullable_int_is_bound_as_null_if_the_argument_value_is_not_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int-nullable"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                IntNullable = null
            });
        }

        [Fact]
        public async Task Property_of_type_nullable_int_array_is_bound_by_parsing_the_argument_values()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int-nullable-array", "3", "15"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                IntNullableArray = new int?[] { 3, 15 }
            });
        }

        [Fact]
        public async Task Property_of_type_nullable_TimeSpan_is_bound_by_parsing_the_argument_value_if_it_is_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--timespan-nullable", "00:14:59"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                TimeSpanNullable = new TimeSpan(00, 14, 59)
            });
        }

        [Fact]
        public async Task Property_of_type_nullable_TimeSpan_is_bound_as_null_if_the_argument_value_is_not_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--timespan-nullable"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                TimeSpanNullable = null
            });
        }

        [Fact]
        public async Task Property_of_a_nullable_enum_type_is_bound_by_parsing_the_argument_value_as_name_if_it_is_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-nullable", "value3"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumNullable = CustomEnum.Value3
            });
        }

        [Fact]
        public async Task Property_of_a_nullable_enum_type_is_bound_by_parsing_the_argument_value_as_id_if_it_is_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-nullable", "3"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumNullable = CustomEnum.Value3
            });
        }

        [Fact]
        public async Task Property_of_a_nullable_enum_type_is_bound_as_null_if_the_argument_value_is_not_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--enum-nullable"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                EnumNullable = null
            });
        }

        [Fact]
        public async Task Property_of_non_nullable_type_can_only_be_bound_if_the_argument_value_is_set()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, _, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--int"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}