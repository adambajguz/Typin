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

            "Hello".Select(c => c.ToConsoleKeyInfo())
                   .ToList()
                   .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            return handler;
        }

        [Fact]
        internal void TestWriteChar()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Assert
            handler.Text.Should().Be("Hello");

            // Act
            " World".Select(c => c.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hello World");
        }

        [Fact]
        public void TestBackspace()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            handler.ReadLine(Backspace);

            // Assert
            handler.Text.Should().Be("Hell");

            // Act
            handler.ReadLine(Backspace);

            // Assert
            handler.Text.Should().Be("Hel");
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hell");

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hel");
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            handler.ReadLine(Delete);

            // Assert
            handler.Text.Should().Be("Hello");
        }

        [Fact]
        public void TestHome()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { Home, 'S'.ToConsoleKeyInfo() }
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("SHello");
        }

        [Fact]
        public void TestEnd()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { Home, End, ExclamationPoint }
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hello!");
        }

        [Fact]
        public void TestLeftArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(LeftArrow)
                .ToList()
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hell No");
        }

        [Fact]
        public void TestRightArrow()
        {
            // Arrange
            LineInputHandler handler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, RightArrow, ExclamationPoint }
                .ForEach((keyInfo) => handler.ReadLine(keyInfo));

            // Assert
            handler.Text.Should().Be("Hello!");
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}
