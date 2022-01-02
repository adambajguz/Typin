namespace TypinExamples.Validation.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class EmailCommandTests
    {
        private readonly ITestOutputHelper _output;

        public EmailCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("t@t.com")]
        [InlineData("test@test.co.uk")]
        [InlineData("a.test@test.co.uk")]
        [InlineData("a.test+0@test.co.uk")]
        [InlineData("admin@x.com")]
        public async Task ShouldRun(string args)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .UseStartup<Startup>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "email -a " + args);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Trim().Should().Be(args);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test@test@co.uk")]
        [InlineData("test.test.co.uk")]
        public async Task ShouldShowError(string args)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .UseStartup<Startup>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, "email -a " + args);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Trim().Should().NotBe(args);
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}
