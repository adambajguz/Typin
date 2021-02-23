namespace TypinExamples.HelloWorld.Tests.CommandTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using TypinExamples.HelloWorld.Commands;
    using Xunit;
    using Xunit.Abstractions;

    public class SimpleCommandTests
    {
        private readonly ITestOutputHelper _output;

        public SimpleCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--age", "11", "--height", "12.0")]
        [InlineData("--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--height", "12.0")]
        public async Task ShouldRun(params string[] args)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .AddCommandsFrom(typeof(SimpleCommand).Assembly);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
