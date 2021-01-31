namespace TypinExamples.HelloWorld.Tests.CommandTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using TypinExamples.HelloWorld.Commands;
    using Xunit;
    using Xunit.Abstractions;

    public class WorldCommandTests
    {
        private readonly ITestOutputHelper _output;

        public WorldCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldRun()
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .AddCommandsFrom(typeof(WorldCommand).Assembly);

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
