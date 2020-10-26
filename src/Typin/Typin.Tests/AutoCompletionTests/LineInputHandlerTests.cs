namespace Typin.Tests.AutoCompletionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Extensions;
    using Xunit;
    using Xunit.Abstractions;
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

        private LineInputHandler GetKeyHandlerInstance()
        {
            // Arrange
            LineInputHandler handler = new LineInputHandler(_console);

            ConsoleKeyInfo[] input = "Hello".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            return handler;
        }

        [Fact]
        public void TestWriteChar()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Assert
            handler.CurrentInput.Should().Be("Hello");

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");
        }

        [Fact]
        public void TestBackspace()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            handler.Read(Backspace);

            // Assert
            handler.CurrentInput.Should().Be("Hell");

            // Act
            handler.Read(Backspace);

            // Assert
            handler.CurrentInput.Should().Be("Hel");
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.Read(keyInfo));

            // Assert
            handler.CurrentInput.Should().Be("Hell");

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.Read(keyInfo));

            // Assert
            handler.CurrentInput.Should().Be("Hel");
        }

        [Fact]
        public void TestCtrlBackspace()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            handler.Read(CtrlBackspace);

            // Assert
            handler.CurrentInput.Should().Be("Hello ");
        }

        [Fact]
        public void TestCtrlBackspaceMultiple()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            handler.Read(CtrlBackspace, CtrlBackspace, CtrlBackspace, CtrlBackspace);

            // Assert
            handler.CurrentInput.Should().BeEmpty();
        }

        [Fact]
        public void TestCtrlDelete()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World  Test".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World  Test");

            // Act
            handler.Read(CtrlLeftArrow, CtrlLeftArrow, CtrlDelete);

            // Assert
            handler.CurrentInput.Should().Be("Hello Test");
        }

        [Fact]
        public void TestCtrlDeleteMultiple()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World  Test".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World  Test");

            // Act
            handler.Read(CtrlLeftArrow, CtrlLeftArrow, CtrlDelete, CtrlDelete, CtrlDelete);

            // Assert
            handler.CurrentInput.Should().Be("Hello ");
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            handler.Read(Delete);

            // Assert
            handler.CurrentInput.Should().Be("Hello");
        }

        [Fact]
        public void TestHome()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { Home, 'S'.ToConsoleKeyInfo() };
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("SHello");
        }

        [Fact]
        public void TestEnd()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { Home, End, ExclamationPoint };
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello!");
        }

        [Fact]
        public void TestLeftArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            handler.Read(LeftArrow, LeftArrow);

            ConsoleKeyInfo[] input = " N".Select(c => c.ToConsoleKeyInfo())
                                         .ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hel Nlo");
        }

        [Fact]
        public void TestRightArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { LeftArrow, LeftArrow, RightArrow, ExclamationPoint };
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hell!o");
        }

        [Fact]
        public void TestCtrlLeftArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            handler.Read(CtrlLeftArrow);

            input = " N".Select(c => c.ToConsoleKeyInfo())
                        .ToArray();

            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello  NWorld");
        }

        [Fact]
        public void TestCtrlRightArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = " World".Select(c => c.ToConsoleKeyInfo()).ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello World");

            // Act
            handler.Read(CtrlLeftArrow, CtrlLeftArrow, CtrlRightArrow);

            input = " N".Select(c => c.ToConsoleKeyInfo())
                        .ToArray();

            handler.Read(input);

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
