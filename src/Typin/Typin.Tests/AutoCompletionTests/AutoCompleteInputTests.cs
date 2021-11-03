namespace Typin.Tests.AutoCompletionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Exceptions;
    using Xunit;

    public sealed class AutoCompleteInputTests : IDisposable
    {
        private readonly string[] _history = new[] { "dotnet run", "git init", "clear" };

        private readonly IConsole _console;
        private readonly MemoryStream stdIn;
        private readonly MemoryStream stdOut;
        private readonly MemoryStream stdErr;

        public AutoCompleteInputTests()
        {
            stdIn = new MemoryStream(Console.InputEncoding.GetBytes("input"));
            stdOut = new MemoryStream();
            stdErr = new MemoryStream();

            _console = new VirtualConsole(input: stdIn,
                                          output: stdOut,
                                          error: stdErr);
        }

        private AutoCompleteInput GetAutoCompleteInstance()
        {
            // Arrange
            AutoCompleteInput instance = new(_console)
            {
                AutoCompletionHandler = new TestAutoCompleteHandler()
            };

            // History should be disabled by default.
            instance.History.IsEnabled.Should().BeFalse();
            instance.History.IsEnabled = true;
            instance.History.IsEnabled.Should().BeTrue();
            instance.History.AddEntries(_history);

            return instance;
        }

        [Fact]
        public async Task Down_arrow_should_return_empty()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(DownArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public async Task Up_arrow_then_escape_should_return_empty()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(UpArrow, Escape, DownArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public async Task Up_then_down_arrow_should_return_last_history_entry()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(UpArrow, DownArrow, Enter);

            // Assert
            text.Should().Be("clear");

            // Act
            text = await input.ReadLineAsync(UpArrow, DownArrow, DownArrow, DownArrow, Enter);

            // Assert
            text.Should().Be("clear");
        }

        [Fact]
        public async Task Up_arrow_should_return_valid_entry()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(UpArrow, UpArrow, UpArrow);

            // Assert
            text.Should().Be("dotnet run");

            // Act
            text = await input.ReadLineAsync(UpArrow, UpArrow, UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().Be("dotnet run");
        }

        [Fact]
        public async Task Should_clear_history()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().Be("dotnet run");

            // Act
            input.History.Clear();
            text = await input.ReadLineAsync(UpArrow, UpArrow, UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public async Task Nothing_should_happen_without_auto_complete_handler()
        {
            // Arrange
            AutoCompleteInput input = new(_console);

            // Act
            string text = await input.ReadLineAsync(Tab, Enter);

            // Assert
            text.Should().BeEmpty();

            // Act
            text = await input.ReadLineAsync(Tab, Tab, Tab);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public async Task Nothing_should_happen_when_no_completions_available()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync('X'.ToConsoleKeyInfo(), Tab);

            // Assert
            text.Should().Be("X");
        }

        [Fact]
        public async Task Should_return_valid_completion_for_W_Tab()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync('W'.ToConsoleKeyInfo(), Tab);

            // Assert
            text.Should().Be("World");
        }

        [Fact]
        public async Task Should_return_valid_completion_for_Tab_and_ShiftTab()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(ShiftTab);

            // Assert
            text.Should().Be("Hello");

            // Act
            text = await input.ReadLineAsync(Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = await input.ReadLineAsync(Tab, ShiftTab);

            // Assert
            text.Should().Be("Hello");
        }

        [Fact]
        public async Task Should_return_valid_completion_for_empty_line()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = await input.ReadLineAsync(Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = await input.ReadLineAsync(Tab, Tab);

            // Assert
            text.Should().Be("Angel");

            // Act
            text = await input.ReadLineAsync(Tab, Tab, Tab);

            // Assert
            text.Should().Be("Hello");

            // Act
            text = await input.ReadLineAsync(Tab, Tab, Tab, Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = await input.ReadLineAsync(Tab, Tab, Tab, Tab, ShiftTab);

            // Assert
            text.Should().Be("Hello");
        }

        [Fact]
        public async Task Custom_shortcut_should_work()
        {
            bool test = false;

            // Arrange
            AutoCompleteInput input = new(_console, new HashSet<ShortcutDefinition>
            {
                new ShortcutDefinition(ConsoleKey.A, ConsoleModifiers.Control, () => { test = true; }),
                new ShortcutDefinition(ConsoleKey.B, ConsoleModifiers.Control, () => { test = true; }),
                new ShortcutDefinition(ConsoleKey.B, () => { test = true; }),
            })
            {
                AutoCompletionHandler = new TestAutoCompleteHandler()
            };

            // History should be disabled by default.
            test.Should().BeFalse();

            // Act
            string text = await input.ReadLineAsync(CtrlA);

            // Assert
            test.Should().BeTrue();
        }

        [Fact]
        public void Duplicated_custom_shortcut_should_throw_exception()
        {
            // Arrange
            Action act = () =>
            {
                AutoCompleteInput input = new(_console, new HashSet<ShortcutDefinition>
                {
                    new ShortcutDefinition(ConsoleKey.A, ConsoleModifiers.Control, () => { }),
                    new ShortcutDefinition(ConsoleKey.Delete, () => { }),
                })
                {
                    AutoCompletionHandler = new TestAutoCompleteHandler()
                };
            };

            // Assert
            act.Should().Throw<TypinException>();
        }

        public void Dispose()
        {
            stdIn.Dispose();
            stdOut.Dispose();
            stdErr.Dispose();
        }
    }
}