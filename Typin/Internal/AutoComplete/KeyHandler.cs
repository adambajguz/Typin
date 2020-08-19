namespace Typin.Internal.AutoComplete
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Typin.Console;

    internal class KeyHandler
    {
        private int _cursorPos;
        private int _cursorLimit;
        private readonly StringBuilder _text = new StringBuilder();

        private readonly LinkedList<string> _history;
        private LinkedListNode<string>? _historyPosition;

        private ConsoleKeyInfo _keyInfo;
        private readonly Dictionary<string, Action> _keyActions;
        private readonly IAutoCompleteHandler? _autoCompleteHandler;
        private string[] _completions = Array.Empty<string>();
        private int _completionStart;
        private int _completionsIndex;
        private readonly IConsole _console;

        public string Text => _text.ToString();
        public bool IsInAutoCompleteMode => _autoCompleteHandler != null && _completions.Length != 0;

        public KeyHandler(IConsole console, LinkedList<string> history, IAutoCompleteHandler? autoCompleteHandler)
        {
            _console = console;
            _history = history;
            _historyPosition = _history.Last;
            _autoCompleteHandler = autoCompleteHandler;

            _keyActions = new Dictionary<string, Action>
            {
                ["LeftArrow"] = MoveCursorLeft,
                ["RightArrow"] = MoveCursorRight,
                ["UpArrow"] = PrevHistory,
                ["DownArrow"] = NextHistory,

                ["Home"] = MoveCursorHome,
                ["End"] = MoveCursorEnd,
                ["Backspace"] = Backspace,
                ["Delete"] = Delete,
                ["Escape"] = ClearLine,

                ["ControlLeftArrow"] = MoveCursorPrevWord,
                ["ControlRightArrow"] = MoveCursorNextWord,
                ["ControlBackspace"] = BackspaceWord,
                ["ControlDelete"] = DeleteWord,

                ["Tab"] = () =>
                {
                    if (IsInAutoCompleteMode)
                    {
                        NextAutoComplete();
                    }
                    else
                    {
                        if (autoCompleteHandler == null || !IsEndOfLine())
                            return;

                        string text = _text.ToString();

                        _completionStart = text.LastIndexOfAny(autoCompleteHandler.Separators);
                        _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                        _completions = autoCompleteHandler.GetSuggestions(text, _completionStart);

                        if (_completions.Length == 0)
                            return;

                        StartAutoComplete();
                    }
                },
                ["ShiftTab"] = () =>
                {
                    if (IsInAutoCompleteMode)
                        PreviousAutoComplete();
                }
            };
        }

        public void Handle(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;

            // If in auto complete mode and Tab wasn't pressed
            if (IsInAutoCompleteMode && _keyInfo.Key != ConsoleKey.Tab)
                ResetAutoComplete();

            _keyActions.TryGetValue(BuildKeyInput(), out Action action);
            action ??= WriteChar;
            action.Invoke();
        }

        private bool IsStartOfLine()
        {
            return _cursorPos == 0;
        }

        private bool IsEndOfLine()
        {
            return _cursorPos == _cursorLimit;
        }

        private bool IsEndOfBuffer()
        {
            return _console.CursorLeft == _console.BufferWidth - 1;
        }

        private void MoveCursorLeft()
        {
            MoveCursorLeft(1);
        }

        private void MoveCursorPrevWord()
        {
            MoveCursorLeft(1);
        }

        private void MoveCursorNextWord()
        {
            MoveCursorLeft(1);
        }

        private void MoveCursorLeft(int count)
        {
            if (count > _cursorPos)
                count = _cursorPos;

            if (count > _console.CursorLeft)
                _console.SetCursorPosition(_console.BufferWidth - 1, _console.CursorTop - 1);
            else
                _console.SetCursorPosition(_console.CursorLeft - count, _console.CursorTop);

            _cursorPos -= count;
        }

        private void MoveCursorHome()
        {
            while (!IsStartOfLine())
                MoveCursorLeft();
        }

        private string BuildKeyInput()
        {
            if (_keyInfo.Modifiers != ConsoleModifiers.Control && _keyInfo.Modifiers != ConsoleModifiers.Shift)
                return _keyInfo.Key.ToString();

            return _keyInfo.Modifiers.ToString() + _keyInfo.Key.ToString();
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine())
                return;

            if (IsEndOfBuffer())
                _console.SetCursorPosition(0, _console.CursorTop + 1);
            else
                _console.SetCursorPosition(_console.CursorLeft + 1, _console.CursorTop);

            ++_cursorPos;
        }

        private void MoveCursorEnd()
        {
            while (!IsEndOfLine())
                MoveCursorRight();
        }

        private void ClearLine()
        {
            MoveCursorEnd();
            Backspace(_cursorPos);
        }

        private void WriteNewString(string str)
        {
            ClearLine();
            foreach (char character in str)
                WriteChar(character);
        }

        private void WriteString(string str)
        {
            foreach (char character in str)
                WriteChar(character);
        }

        private void WriteChar()
        {
            WriteChar(_keyInfo.KeyChar);
        }

        private void WriteChar(char c)
        {
            if (IsEndOfLine())
            {
                _text.Append(c);
                _console.Output.Write(c.ToString());
                _cursorPos++;
            }
            else
            {
                int left = _console.CursorLeft;
                int top = _console.CursorTop;
                string str = _text.ToString().Substring(_cursorPos);
                _text.Insert(_cursorPos, c);
                _console.Output.Write(c.ToString() + str);
                _console.SetCursorPosition(left, top);
                MoveCursorRight();
            }

            _cursorLimit++;
        }

        private void BackspaceWord()
        {

        }

        private void Backspace()
        {
            Backspace(1);
        }

        private void Backspace(int count)
        {
            if (count > _cursorPos)
                count = _cursorPos;

            MoveCursorLeft(count);
            int index = _cursorPos;
            _text.Remove(index, count);

            string replacement = _text.ToString().Substring(index);
            int left = _console.CursorLeft;
            int top = _console.CursorTop;

            string spaces = new string(' ', count);
            _console.Output.Write(string.Format("{0}{1}", replacement, spaces));
            _console.SetCursorPosition(left, top);
            _cursorLimit -= count;
        }

        private void DeleteWord()
        {

        }

        private void Delete()
        {
            if (IsEndOfLine())
                return;

            int index = _cursorPos;
            _text.Remove(index, 1);

            string replacement = _text.ToString().Substring(index);
            int left = _console.CursorLeft;
            int top = _console.CursorTop;
            _console.Output.Write(string.Format("{0} ", replacement));
            _console.SetCursorPosition(left, top);
            _cursorLimit--;
        }

        private void StartAutoComplete()
        {
            Backspace(_cursorPos - _completionStart);

            _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        private void NextAutoComplete()
        {
            Backspace(_cursorPos - _completionStart);

            _completionsIndex++;

            if (_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        private void PreviousAutoComplete()
        {
            Backspace(_cursorPos - _completionStart);

            _completionsIndex--;

            if (_completionsIndex == -1)
                _completionsIndex = _completions.Length - 1;

            WriteString(_completions[_completionsIndex]);
        }

        private void PrevHistory()
        {
            if (_historyPosition is null)
                _historyPosition = _history.Last;

            if (_historyPosition?.Previous is LinkedListNode<string> node)
            {
                WriteNewString(node.Value);
                _historyPosition = node;
            }
        }

        private void NextHistory()
        {
            if (_historyPosition is null)
                _historyPosition = _history.Last;

            if (_historyPosition?.Next is LinkedListNode<string> node)
            {
                _historyPosition = node;
                WriteNewString(node.Value);
            }
        }

        private void ResetAutoComplete()
        {
            _completions = Array.Empty<string>();
            _completionsIndex = 0;
        }
    }
}
