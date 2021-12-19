namespace Typin.Tests.ArgumentTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;
    using Typin.Tests.Data.Common.Extensions;
    using Typin.Tests.Data.Valid.Commands;

    public class UnsupportedArgumentConversionTests
    {
        private readonly ITestOutputHelper _output;

        public UnsupportedArgumentConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("--str-non-initializable-class")]
        [InlineData("--str-non-initializable-struct")]
        [InlineData("--str-enumerable-non-initializable")]
        public async Task Property_of_custom_type_must_be_string_initializable_in_order_to_be_bound(string optionName)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<UnsupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                nameof(UnsupportedArgumentTypesCommand), optionName, "foobar"
            });

            // Assert
            exitCode.Should().NotBe(ExitCode.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().Contain("Can't convert");
        }
    }
}