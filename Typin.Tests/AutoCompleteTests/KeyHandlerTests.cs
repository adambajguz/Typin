namespace Typin.Tests.AutoCompleteTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Extensions;
    using Typin.Internal.AutoComplete;
    using Xunit;
    using static Typin.Extensions.ConsoleKeyInfoExtensions;

    public sealed class KeyHandlerTests : IDisposable
    {
        private readonly LinkedList<string> _history = new LinkedList<string>(new string[] { "dotnet run", "git init", "clear" });
        private readonly TestAutoCompleteHandler _autoCompleteHandler;
        private readonly string[] _completions;

        private readonly IConsole _console;
        private readonly MemoryStream stdIn;
        private readonly MemoryStream stdOut;
        private readonly MemoryStream stdErr;

        public KeyHandlerTests()
        {
            _autoCompleteHandler = new TestAutoCompleteHandler();
            _completions = _autoCompleteHandler.GetSuggestions("", 0);

            stdIn = new MemoryStream(Console.InputEncoding.GetBytes("input"));
            stdOut = new MemoryStream();
            stdErr = new MemoryStream();

            _console = new VirtualConsole(input: stdIn,
                                          output: stdOut,
                                          error: stdErr);
        }

        private KeyHandler GetKeyHandlerInstanceForTests(TestAutoCompleteHandler? _autoCompleteHandler = null)
        {
            // Arrange
            KeyHandler keyHandler = new KeyHandler(_console, _history, _autoCompleteHandler);

            "Hello".Select(c => c.ToConsoleKeyInfo())
                   .ToList()
                   .ForEach(keyHandler.Handle);

            return keyHandler;
        }

        [Fact]
        internal void TestWriteChar()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            _keyHandler.Handle(Delete);

            // Assert
            _keyHandler.Text.Should().Be("Hello");
        }

        [Fact]
        public void TestHome()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

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
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            new List<ConsoleKeyInfo>() { LeftArrow, RightArrow, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            // Assert
            _keyHandler.Text.Should().Be("Hello!");
        }

        [Fact]
        public void TestUpArrow()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            // Assert
            _history.AsEnumerable().Reverse().ToList().ForEach((history) =>
            {
                _keyHandler.Handle(UpArrow);
                _keyHandler.Text.Should().Be(history);
            });
        }

        [Fact]
        public void TestDownArrow()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            Enumerable.Repeat(UpArrow, _history.Count)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            // Assert
            _history.ToList().ForEach(history =>
            {
                _keyHandler.Text.Should().Be(history);
                _keyHandler.Handle(DownArrow);
            });
        }

        [Fact]
        public void TestTab()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            _keyHandler.Handle(Tab);

            // Assert
            // Nothing happens when no auto complete handler is set
            _keyHandler.Text.Should().Be("Hello");

            // Arrange
            _keyHandler = GetKeyHandlerInstanceForTests(_autoCompleteHandler);

            // Act
            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            // Assert
            _completions.ToList().ForEach(completion =>
            {
                _keyHandler.Handle(Tab);
                _keyHandler.Text.Should().Be($"HelloHi {completion}");
            });
        }

        [Fact]
        public void TestShiftTab()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            _keyHandler.Handle(Tab);

            // Assert
            // Nothing happens when no auto complete handler is set
            _keyHandler.Text.Should().Be("Hello");

            // Arrange
            _keyHandler = GetKeyHandlerInstanceForTests(_autoCompleteHandler);

            // Act
            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            // Bring up the first Autocomplete
            _keyHandler.Handle(Tab);

            // Assert
            _completions.Reverse().ToList().ForEach(completion =>
            {
                _keyHandler.Handle(ShiftTab);
                _keyHandler.Text.Should().Be($"HelloHi {completion}");
            });
        }

        [Fact]
        public void MoveCursorThenPreviousHistory()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            _keyHandler.Handle(LeftArrow);
            _keyHandler.Handle(UpArrow);

            // Assert
            _keyHandler.Text.Should().Be("git init");
        }

        [Fact]
        public void PreviousThenMoveCursorThenNextHistory()
        {
            // Arrange
            KeyHandler _keyHandler = GetKeyHandlerInstanceForTests();

            // Act
            _keyHandler.Handle(UpArrow);

            // Assert
            _keyHandler.Text.Should().Be("git init");

            // Act
            _keyHandler.Handle(LeftArrow);
            _keyHandler.Handle(DownArrow);

            // Assert
            _keyHandler.Text.Should().Be("clear");
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}
