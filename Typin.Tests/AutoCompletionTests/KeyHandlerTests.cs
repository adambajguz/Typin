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

    public sealed class KeyHandlerTests : IDisposable
    {
        private readonly IConsole _console;
        private readonly MemoryStream stdIn;
        private readonly MemoryStream stdOut;
        private readonly MemoryStream stdErr;

        public KeyHandlerTests()
        {
            stdIn = new MemoryStream(Console.InputEncoding.GetBytes("input"));
            stdOut = new MemoryStream();
            stdErr = new MemoryStream();

            _console = new VirtualConsole(input: stdIn,
                                          output: stdOut,
                                          error: stdErr);
        }

        private KeyHandler GetKeyHandlerInstance()
        {
            // Arrange
            KeyHandler keyHandler = new KeyHandler(_console);

            "Hello".Select(c => c.ToConsoleKeyInfo())
                   .ToList()
                   .ForEach(keyHandler.Handle);

            return keyHandler;
        }

        [Fact]
        internal void TestWriteChar()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Assert
            _keyHandler.Text.Should().Be("Hello");

            // Act
            " World".Select(c => c.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hello World");
        }

        [Fact]
        public void TestBackspace()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            _keyHandler.Handle(Backspace);

            // Assert
            _keyHandler.Text.Should().Be("Hell");

            // Act
            _keyHandler.Handle(Backspace);

            // Assert
            _keyHandler.Text.Should().Be("Hel");
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hell");

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hel");
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            _keyHandler.Handle(Delete);

            // Assert
            _keyHandler.Text.Should().Be("Hello");
        }

        [Fact]
        public void TestHome()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { Home, 'S'.ToConsoleKeyInfo() }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("SHello");
        }

        [Fact]
        public void TestEnd()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { Home, End, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hello!");
        }

        [Fact]
        public void TestLeftArrow()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(LeftArrow)
                .ToList()
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hell No");
        }

        [Fact]
        public void TestRightArrow()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstance();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, RightArrow, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hello!");
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}
