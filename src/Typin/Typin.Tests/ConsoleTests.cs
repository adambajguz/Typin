namespace Typin.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CliWrap;
    using CliWrap.Buffered;
    using FluentAssertions;
    using Typin.Console;
    using Xunit;
    using Xunit.Abstractions;

    public class ConsoleTests
    {
        private readonly ITestOutputHelper _output;

        public ConsoleTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Real_implementation_of_console_maps_directly_to_system_console()
        {
            // Arrange
            var command = "Hello world" | Cli.Wrap("dotnet")
                .WithArguments(a => a
                    .Add(Dummy.Program.Location)
                    .Add("console-test"));

            // Act
            BufferedCommandResult result = await command.ExecuteBufferedAsync();

            // Assert
            result.StandardOutput.TrimEnd().Should().Be("Hello world");
            result.StandardError.TrimEnd().Should().Be("Hello world");
        }

        [Fact]
        public void Real_implementation_of_console_can_be_used_to_execute_commands()
        {
            // Arrange
            using IConsole console = new SystemConsole();

            // Act
            console.ResetColor();
            console.ForegroundColor = ConsoleColor.DarkMagenta;
            console.BackgroundColor = ConsoleColor.DarkMagenta;

            // Assert
            console.ForegroundColor.Should().Be(Console.ForegroundColor);
            console.BackgroundColor.Should().Be(Console.BackgroundColor);
        }

        [Fact]
        public void Fake_implementation_of_console_can_be_used_to_execute_commands_in_isolation()
        {
            // Arrange
            using var stdIn = new MemoryStream(Console.InputEncoding.GetBytes("input"));
            using var stdOut = new MemoryStream();
            using var stdErr = new MemoryStream();

            using IConsole console = new VirtualConsole(input: stdIn,
                                                        output: stdOut,
                                                        error: stdErr);

            // Act
            console.Output.Write("output");
            console.Error.Write("error");

            var stdInData = console.Input.ReadToEnd();
            var stdOutData = console.Output.Encoding.GetString(stdOut.ToArray());
            var stdErrData = console.Error.Encoding.GetString(stdErr.ToArray());

            console.Clear();
            console.ResetColor();
            console.ForegroundColor = ConsoleColor.DarkMagenta;
            console.BackgroundColor = ConsoleColor.DarkMagenta;
            console.CursorLeft = 42;
            console.CursorTop = 24;
            console.BufferHeight = 80;
            console.BufferWidth = 120;
            console.WindowWidth = 45;
            console.WindowHeight = 70;

            console.SetCursorPosition(24, 42);

            // Assert
            stdInData.Should().Be("input");
            stdOutData.Should().Be("output");
            stdErrData.Should().Be("error");

            console.Input.Should().NotBeSameAs(Console.In);
            console.Output.Should().NotBeSameAs(Console.Out);
            console.Error.Should().NotBeSameAs(Console.Error);

            console.IsInputRedirected.Should().BeTrue();
            console.IsOutputRedirected.Should().BeTrue();
            console.IsErrorRedirected.Should().BeTrue();

            console.ForegroundColor.Should().NotBe(Console.ForegroundColor);
            console.BackgroundColor.Should().NotBe(Console.BackgroundColor);
        }
    }
}