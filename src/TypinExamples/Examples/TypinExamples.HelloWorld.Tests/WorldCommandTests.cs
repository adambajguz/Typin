namespace TypinExamples.HelloWorld.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using Typin.Console;
    using TypinExamples.HelloWorld.Commands;
    using Xunit;

    public class WorldCommandTests
    {
        [Fact]
        public async Task Should_run()
        {
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WorldCommand>()
                .UseConsole(console)
                .Build();

            var args = new string[] { };
            var envVars = new Dictionary<string, string>();

            // Act
            int exitCode = await app.RunAsync(args, envVars);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
