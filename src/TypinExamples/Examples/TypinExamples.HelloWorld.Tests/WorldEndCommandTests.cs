namespace TypinExamples.HelloWorld.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using Typin.Console;
    using TypinExamples.HelloWorld.Commands;
    using Xunit;

    public class WorldEndCommandTests
    {
        [Theory]
        [InlineData("world end", "05/11/2020 07:22:16", "--CONFIRM", "false", "-f")]
        [InlineData("world end", "05/11/2020 07:22:16", "--CONFIRM", "true", "-f")]
        [InlineData("world end", "05/11/2020 07:22:16", "--CONFIRM", "false")]
        [InlineData("world end", "05/11/2020 07:22:16", "--CONFIRM", "true")]
        [InlineData("world end", "05/11/2020", "--CONFIRM", "true")]
        [InlineData("world end", "05/11/2020", "--CONFIRM", "false")]
        [InlineData("world end", "05 /11/2020 07:22:16", "--CONFIRM", "true", "-f", "false")]
        [InlineData("world end", "05 /11/2020 07:22:16", "--CONFIRM", "false", "-f", "true")]
        [InlineData("world end", "05 /11/2020 07:22:16", "--CONFIRM", "false", "-f", "false")]
        [InlineData("world end", "05 /11/2020 07:22:16", "--CONFIRM", "true", "-f", "true")]
        public async Task Should_run(params string[] args)
        {
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WorldEndCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(args, new Dictionary<string, string>());

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}