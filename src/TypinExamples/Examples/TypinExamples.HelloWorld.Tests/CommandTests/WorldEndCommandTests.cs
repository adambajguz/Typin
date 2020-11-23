namespace TypinExamples.HelloWorld.Tests.CommandTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using Typin.Console;
    using TypinExamples.ExamplesTests.Common.Extensions;
    using TypinExamples.HelloWorld.Commands;
    using Xunit;
    using Xunit.Abstractions;

    public class WorldEndCommandTests
    {
        private readonly ITestOutputHelper _output;

        public WorldEndCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private const string COMMAND_NAME = "world end";

        [Theory]
        [InlineData(COMMAND_NAME, "05/11/2020 07:22:16", "--CONFIRM", "false", "-f")]
        [InlineData(COMMAND_NAME, "05/11/2020 07:22:16", "--CONFIRM", "true", "-f")]
        [InlineData(COMMAND_NAME, "05/11/2020 07:22:16", "--CONFIRM", "false")]
        [InlineData(COMMAND_NAME, "05/11/2020 07:22:16", "--CONFIRM", "true")]
        [InlineData(COMMAND_NAME, "05/11/2020", "--CONFIRM", "true")]
        [InlineData(COMMAND_NAME, "05/11/2020", "--CONFIRM", "false")]
        [InlineData(COMMAND_NAME, "05 /11/2020 07:22:16", "--CONFIRM", "true", "-f", "false")]
        [InlineData(COMMAND_NAME, "05 /11/2020 07:22:16", "--CONFIRM", "false", "-f", "true")]
        [InlineData(COMMAND_NAME, "05 /11/2020 07:22:16", "--CONFIRM", "false", "-f", "false")]
        [InlineData(COMMAND_NAME, "05 /11/2020 07:22:16", "--CONFIRM", "true", "-f", "true")]
        public async Task ShouldRun(params string[] args)
        {
            //Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WorldEndCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}