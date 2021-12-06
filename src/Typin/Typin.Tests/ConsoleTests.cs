namespace Typin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CliWrap;
    using CliWrap.Buffered;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.Console;
    using Typin.Tests.Extensions;
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
            SystemConsole console = new();

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
            using MemoryStream stdIn = new(Console.InputEncoding.GetBytes("input"));
            using MemoryStream stdOut = new();
            using MemoryStream stdErr = new();

            using VirtualConsole console = new(input: stdIn,
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

            console.Input.IsRedirected.Should().BeTrue();
            console.Output.IsRedirected.Should().BeTrue();
            console.Error.IsRedirected.Should().BeTrue();

            console.ForegroundColor.Should().NotBe(Console.ForegroundColor);
            console.BackgroundColor.Should().NotBe(Console.BackgroundColor);
        }

        [Fact]
        public async Task Console_color_extensions_should_work()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithColorsCommand>();

            var (console, stdOut, stdErr) = VirtualAnsiConsole.CreateBuffered(isInputRedirected: false, isOutputRedirected: false, isErrorRedirected: false);

            CliApplication application = builder.UseConsole(console)
                                                .Build();

            int exitCode = await application.RunAsync("colors", new Dictionary<string, string>());

            _output.WriteLine("Exit Code: {0}", exitCode);
            _output.Print(stdOut, stdErr);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain(WithColorsCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Console_color_extensions_should_work_after_colors_reset()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<WithColorsAndResetCommand>();

            var (console, stdOut, stdErr) = VirtualAnsiConsole.CreateBuffered(isInputRedirected: false, isOutputRedirected: false, isErrorRedirected: false);

            CliApplication application = builder.UseConsole(console)
                                                .Build();

            int exitCode = await application.RunAsync("colors-with-reset", new Dictionary<string, string>());

            _output.WriteLine("Exit Code: {0}", exitCode);
            _output.Print(stdOut, stdErr);

            // Assert
            exitCode.Should().Be(ExitCode.Success);
            stdOut.GetString().Should().Contain(WithColorsAndResetCommand.ExpectedOutputText);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }
    }
}