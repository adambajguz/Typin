namespace Typin.AutoCompletion
{
    using System;
    using System.Collections.Generic;
    using Typin.Console;

    internal class AutoCompleteInput
    {
        private readonly IConsole _console;
        private readonly KeyHandler _keyHandler;

        public InputHistoryProvider History { get; }
        public IAutoCompleteHandler? AutoCompletionHandler { get; set; }

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
                    {
                        NextAutoComplete();
                    }
                    else
                    {
                        if (AutoCompletionHandler is null || !_keyHandler.IsEndOfLine)
                            return;

                        string text = _keyHandler.Text;

                        _completionStart = text.LastIndexOfAny(AutoCompletionHandler.Separators);
                        _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                        _completions = AutoCompletionHandler.GetSuggestions(text, _completionStart);

                        if (_completions.Length == 0)
                            return;

                        StartAutoComplete();
                    }
                },
                ["ShiftTab"] = () =>
                {
                    if (IsInAutoCompleteMode)
                        PreviousAutoComplete();
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
                History.AddEntry(text);
                History.ResetSelection();
            }

            return text;
        }

        private void StartAutoComplete()
        {
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            _completionsIndex = 0;

            _keyHandler.Write(_completions[_completionsIndex]);
        }

        private void NextAutoComplete()
        {
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            _completionsIndex++;

            if (_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            _keyHandler.Write(_completions[_completionsIndex]);
        }

        private void PreviousAutoComplete()
        {
            _keyHandler.Backspace(_keyHandler.CursorPosition - _completionStart);

            _completionsIndex--;

            if (_completionsIndex == -1)
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
