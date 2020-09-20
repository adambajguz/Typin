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
            ConsoleKeyInfo[] input = " N".Select(c => c.ToConsoleKeyInfo())
                                         .Prepend(LeftArrow)
                                         .ToArray();
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hell No");
        }

        [Fact]
        public void TestRightArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            ConsoleKeyInfo[] input = new ConsoleKeyInfo[] { LeftArrow, RightArrow, ExclamationPoint };
            handler.Read(input);

            // Assert
            handler.CurrentInput.Should().Be("Hello!");
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}
