namespace Typin.Tests.AutoCompletionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Extensions;
    using Xunit;
    using static Typin.Extensions.ConsoleKeyInfoExtensions;

    public sealed class LineInputHandlerTests : IDisposable
    {
        private readonly IConsole _console;
        private readonly MemoryStream stdIn;
        private readonly MemoryStream stdOut;
        private readonly MemoryStream stdErr;

        public LineInputHandlerTests()
        {
            stdIn = new MemoryStream(Console.InputEncoding.GetBytes("input"));
            stdOut = new MemoryStream();
            stdErr = new MemoryStream();

            _console = new VirtualConsole(input: stdIn,
                                          output: stdOut,
                                          error: stdErr);
        }

        private async Task<LineInputHandler> GetKeyHandlerInstance()
        {
            // Arrange
            LineInputHandler handler = new LineInputHandler(_console);

            ConsoleKeyInfo[] input = "Hello".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            return handler;
        }

        [Fact]
        public async Task TestWriteChar()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Assert
            handler.CurrentInput.Should().Be("Hello");

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");
        }

        [Fact]
        public async Task TestBackspace()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            await handler.ReadAsync(Backspace);

            // Assert
            handler.CurrentInput.Should().Be("Hell");

            // Act
            await handler.ReadAsync(Backspace);

            // Assert
            handler.CurrentInput.Should().Be("Hel");
        }

        [Fact]
        public async Task TestDelete()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.ReadAsync(keyInfo).Wait());

            // Assert
            handler.CurrentInput.Should().Be("Hell");

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.ReadAsync(keyInfo).Wait());

            // Assert
            handler.CurrentInput.Should().Be("Hel");
        }

        [Fact]
        public async Task TestCtrlBackspace()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            await handler.ReadAsync(CtrlBackspace);

            // Assert
            handler.CurrentInput.Should().Be("Hello ");
        }

        [Fact]
        public async Task TestCtrlBackspaceMultiple()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            await handler.ReadAsync(CtrlBackspace, CtrlBackspace, CtrlBackspace, CtrlBackspace);

            // Assert
            handler.CurrentInput.Should().BeEmpty();
        }

        [Fact]
        public async Task TestCtrlDelete()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World  Test".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World  Test");

            // Act
            await handler.ReadAsync(CtrlLeftArrow, CtrlLeftArrow, CtrlDelete);

            // Assert
            handler.CurrentInput.Should().Be("Hello Test");
        }

        [Fact]
        public async Task TestCtrlDeleteMultiple()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World  Test".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World  Test");

            // Act
            await handler.ReadAsync(CtrlLeftArrow, CtrlLeftArrow, CtrlDelete, CtrlDelete, CtrlDelete);

            // Assert
            handler.CurrentInput.Should().Be("Hello ");
        }

        [Fact]
        public async Task TestDelete_EndOfLine()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            await handler.ReadAsync(Delete);

            // Assert
            handler.CurrentInput.Should().Be("Hello");
        }

        [Fact]
        public async Task TestHome()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { Home, 'S'.ToConsoleKeyInfo() };
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("SHello");
        }

        [Fact]
        public async Task TestEnd()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { Home, End, ExclamationPoint };
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello!");
        }

        [Fact]
        public async Task TestLeftArrow()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            await handler.ReadAsync(LeftArrow, LeftArrow);

            ConsoleKeyInfo[] input = " N".Select(c => c.ToConsoleKeyInfo())
                                         .ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hel Nlo");
        }

        [Fact]
        public async Task TestRightArrow()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { LeftArrow, LeftArrow, RightArrow, ExclamationPoint };
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hell!o");
        }

        [Fact]
        public async Task TestCtrlLeftArrow()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            await handler.ReadAsync(CtrlLeftArrow);

            input = " N".Select(c => c.ToConsoleKeyInfo())
                        .ToArray();

            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello  NWorld");
        }

        [Fact]
        public async Task TestCtrlRightArrow()
        {
            // Arrange
            LineInputHandler handler = await GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            await handler.ReadAsync(CtrlLeftArrow, CtrlLeftArrow, CtrlRightArrow);

            input = " N".Select(c => c.ToConsoleKeyInfo())
                        .ToArray();

            await handler.ReadAsync(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello  NWorld");
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}
