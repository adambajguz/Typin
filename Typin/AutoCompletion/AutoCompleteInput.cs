namespace Typin.AutoCompletion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Console;
    using Typin.Extensions;

    internal class AutoCompleteInput
    {
        private readonly IConsole _console;
        private readonly KeyHandler _keyHandler;

        public InputHistoryProvider History { get; }
        public IAutoCompletionHandler? AutoCompletionHandler { get; set; }

        private string[] _completions = Array.Empty<string>();
        private int _completionStart;
        private int _completionsIndex;

        public bool IsInAutoCompleteMode => AutoCompletionHandler != null && _completions.Length != 0;

        /// <summary>
        /// Initializes an instance of <see cref="AutoCompleteInput"/>.
        /// </summary>
        public AutoCompleteInput(IConsole console)
        {
            _console = console;
            History = new InputHistoryProvider();

            var keyActions = new Dictionary<string, Action>
            {
                // History
                ["UpArrow"] = () =>
                {
                    if (History.SelectionUp())
                    {
                        _keyHandler.ClearLine();
                        _keyHandler.Write(History.GetSelection());
                    }
                },
                ["DownArrow"] = () =>
                {
                    if (History.SelectionDown())
                    {
                        _keyHandler.ClearLine();
                        _keyHandler.Write(History.GetSelection());
                    }
                },

                // AutoComplete
                ["Tab"] = () =>
                {
                    if (IsInAutoCompleteMode)
                        NextAutoComplete();
                    else
                        InitAutoComplete();
                },
                ["ShiftTab"] = () =>
                {
                    if (IsInAutoCompleteMode)
                        PreviousAutoComplete();
                    else
                        InitAutoComplete(true);
                },

                ["Escape"] = () =>
                {
                    _keyHandler.ClearLine();
                    History.ResetSelection();
                    ResetAutoComplete();
                }
            };

            _keyHandler = new KeyHandler(console, keyActions);
        }

        public string ReadLine()
        {
            //Get line
            {
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = _console.ReadKey(true);
                    _keyHandler.Handle(keyInfo);

                } while (keyInfo.Key != ConsoleKey.Enter);

                _console.Output.WriteLine();
            }

            string text = _keyHandler.Text.TrimEnd('\n', '\r');
            _keyHandler.Reset();
            ResetAutoComplete();

            if (History.IsEnabled)
            {
                History.TryAddEntry(text);
                History.ResetSelection();
            }

            return text;
        }

        public string ReadLine(params ConsoleKeyInfo[] line)
        {
            foreach (var keyInfo in line)
            {
                _keyHandler.Handle(keyInfo);
            }

            if (line.LastOrDefault().Key != ConsoleKey.Enter)
                _keyHandler.Handle(ConsoleKeyInfoExtensions.Enter);

            _console.Output.WriteLine();

            string text = _keyHandler.Text.TrimEnd('\n', '\r');
            _keyHandler.Reset();
            ResetAutoComplete();

            if (History.IsEnabled)
            {
                History.TryAddEntry(text);
                History.ResetSelection();
            }

            return text;
        }

        private void InitAutoComplete(bool fromEnd = false)
        {
            if (AutoCompletionHandler is null || !_keyHandler.IsEndOfLine)
                return;

            string text = _keyHandler.Text;

            _completionStart = text.LastIndexOfAny(AutoCompletionHandler.Separators);
            _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

            _completions = AutoCompletionHandler.GetSuggestions(text, _completionStart);

            if (_completions.Length == 0)
                return;

            //StartAutoComplete;
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            _completionsIndex = fromEnd ? _completions.Length - 1 : 0;

            _keyHandler.Write(_completions[_completionsIndex]);
        }

        private void NextAutoComplete()
        {
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            if (++_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            _keyHandler.Write(_completions[_completionsIndex]);
        }

        private void PreviousAutoComplete()
        {
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            if (--_completionsIndex == -1)
                _completionsIndex = _completions.Length - 1;

            _keyHandler.Write(_completions[_completionsIndex]);
        }

        private void ResetAutoComplete()
        {
            _completions = Array.Empty<string>();
            _completionsIndex = 0;
        }
    }
}
