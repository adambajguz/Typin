using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Typin;
using Typin.Console;
using TypinExamples.HelloWorld.Commands;
using Xunit;

namespace TypinExamples.HelloWorld.Tests
{
    public class UnitTest_SimpleCommand
    {
        [Theory]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--name", "test_name", "--surname", "test_surname", "--age", "11", "--height", "12.0")]
        [InlineData("--mail", "test_mail", "--age", "11", "--height", "12.0")]
        [InlineData("--height", "12.0")]
        public async Task ConcatCommand_Test(params string[] args)
        {

            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<SimpleCommand>()
                .UseConsole(console)
                .Build();

            var envVars = new Dictionary<string, string>();

            // Act
            int exitCode = await app.RunAsync(args, envVars);

            // Assert
            exitCode.Should().Be(0);
            stdOut.GetString().Should().NotBeNullOrWhiteSpace();
            stdErr.GetString().Should().BeNullOrWhiteSpace();

        }
    }
    public class UnitTest_WorldCommand
    {
        [Fact]
        [InlineData]
        public async Task ConcatCommand_Test()
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
public class UnitTest_WorldEndCommand
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
    public async Task ConcatCommand_Test(params string[] args)
    {

        var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

        var app = new CliApplicationBuilder()
            .AddCommand<WorldEndCommand>()
            .UseConsole(console)
            .Build();

        var envVars = new Dictionary<string, string>();

        // Act
        int exitCode = await app.RunAsync(args, envVars);

        // Assert
        exitCode.Should().Be(0);
        stdOut.GetString().Should().NotBeNullOrWhiteSpace();
        stdErr.GetString().Should().BeNullOrWhiteSpace();

    }
}