namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Common.Help;
    using Typin.Tests.Data.Valid.Commands;
    using Xunit;
    using Xunit.Abstractions;
    using Typin.Tests.Data.Common.Extensions;

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
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[] { nameof(WithParametersCommand), "--help" });

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain(TestHelpWriter.ExpectedStringOnStandardWrite);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}