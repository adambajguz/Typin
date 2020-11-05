namespace TypinExamples.HelloWorld.Tests
{
    using System;
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

            // Act
            int exitCode = await app.RunAsync(Array.Empty<string>(), new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}
