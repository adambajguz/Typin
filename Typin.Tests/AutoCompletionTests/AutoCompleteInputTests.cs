namespace Typin.Tests.AutoCompletionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Extensions;
    using Xunit;
    using static Typin.Extensions.ConsoleKeyInfoExtensions;

    public sealed class AutoCompleteInputTests : IDisposable
    {
        private readonly string[] _history = new string[] { "dotnet run", "git init", "clear" };

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
            AutoCompleteInput instance = new AutoCompleteInput(_console)
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
        public void Down_arrow_should_return_empty()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(DownArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public void Up_arrow_then_escape_should_return_empty()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(UpArrow, Escape, DownArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public void Up_then_down_arrow_should_return_last_history_entry()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(UpArrow, DownArrow, Enter);

            // Assert
            text.Should().Be("clear");

            // Act
            text = input.ReadLine(UpArrow, DownArrow, DownArrow, DownArrow, Enter);

            // Assert
            text.Should().Be("clear");
        }

        [Fact]
        public void Up_arrow_should_return_valid_entry()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(UpArrow, UpArrow, UpArrow);

            // Assert
            text.Should().Be("dotnet run");

            // Act
            text = input.ReadLine(UpArrow, UpArrow, UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().Be("dotnet run");
        }

        [Fact]
        public void Should_clear_history()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().Be("dotnet run");

            // Act
            input.History.Clear();
            text = input.ReadLine(UpArrow, UpArrow, UpArrow, UpArrow, UpArrow, Enter);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public void Nothing_should_happen_without_auto_complete_handler()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(_console);

            // Act
            string text = input.ReadLine(Tab, Enter);

            // Assert
            text.Should().BeEmpty();

            // Act
            text = input.ReadLine(Tab, Tab, Tab);

            // Assert
            text.Should().BeEmpty();
        }

        [Fact]
        public void Should_return_valid_completion_for_W_Tab()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine('W'.ToConsoleKeyInfo(), Tab);

            // Assert
            text.Should().Be("World");
        }

        [Fact]
        public void Should_return_valid_completion_for_Tab_and_ShiftTab()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(ShiftTab);

            // Assert
            text.Should().Be("Hello");

            // Act
            text = input.ReadLine(Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = input.ReadLine(Tab, ShiftTab);

            // Assert
            text.Should().Be("Hello");
        }

        [Fact]
        public void Should_return_valid_completion_for_empty_line()
        {
            // Arrange
            AutoCompleteInput input = GetAutoCompleteInstance();

            // Act
            string text = input.ReadLine(Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = input.ReadLine(Tab, Tab);

            // Assert
            text.Should().Be("Angel");

            // Act
            text = input.ReadLine(Tab, Tab, Tab);

            // Assert
            text.Should().Be("Hello");

            // Act
            text = input.ReadLine(Tab, Tab, Tab, Tab);

            // Assert
            text.Should().Be("World");

            // Act
            text = input.ReadLine(Tab, Tab, Tab, Tab, ShiftTab);

            // Assert
            text.Should().Be("Hello");
        }

        [Fact]
        public void Custom_shortcut_should_work()
        {
            bool test = false;

            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(_console, new HashSet<ShortcutDefinition>
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
            string text = input.ReadLine(CtrlA);

            // Assert
            test.Should().BeTrue();
        }

        [Fact]
        public void Duplicated_custom_shortcut_should_throw_exception()
        {
            // Arrange
            Action act = () =>
            {
                AutoCompleteInput input = new AutoCompleteInput(_console, new HashSet<ShortcutDefinition>
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