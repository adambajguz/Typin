namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.Help;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class CustomHelpWriterTests
    {
        private readonly ITestOutputHelper _output;

        public CustomHelpWriterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Application_should_use_custom_help_writer_when_registered()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .UseHelpWriter<TestHelpWriter>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--help" });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().Contain(TestHelpWriter.ExpectedStringOnStandardWrite);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Application_should_use_custom_help_writer_when_registered_after_exception()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithExceptionThatPrintsHelpCommand>()
                .UseHelpWriter<TestHelpWriter>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { "cmd", "--msg", "Error Message Test" });

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            stdOut.GetString().Should().Contain(TestHelpWriter.ExpectedStringOnExceptionWrite);
            stdErr.GetString().Should().Contain("Error Message Test");
        }
    }
}