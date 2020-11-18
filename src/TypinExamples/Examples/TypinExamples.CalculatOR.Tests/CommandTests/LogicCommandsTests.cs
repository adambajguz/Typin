namespace TypinExamples.CalculatOR.Tests.CommandTests.Logic
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin;
    using Typin.Console;
    using TypinExamples.CalculatOR.Commands.Arithmetic;
    using Xunit;

    public class LogicCommandsTests
    {
        [Theory]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--age", "11", "--height", "12.0")]
        [InlineData("--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--height", "12.0")]
        public async Task Should_run(params string[] args)
        {
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<AddCommand>()
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
