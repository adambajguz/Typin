namespace Typin.AutoCompletion
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Typin.Console;
    using Typin.Exceptions;

    internal sealed class KeyHandler
    {
        private readonly IConsole _console;
        private readonly Dictionary<string, Action> _keyActions;
        private readonly StringBuilder _text = new StringBuilder();

        /// <summary>
        /// Cursor position relative to input.
        /// </summary>
        public int CursorPosition { get; private set; }

        /// <summary>
        /// Last console key info.
        /// </summary>
        public ConsoleKeyInfo LastKey { get; private set; }

        public bool IsStartOfLine => CursorPosition == 0;
        public bool IsEndOfLine => CursorPosition == _text.Length;
        public bool IsEndOfBuffer => _console.CursorLeft == _console.BufferWidth - 1;

        /// <summary>
        /// Current input text.
        /// </summary>
        public string Text => _text.ToString();

        /// <summary>
        /// Initializes an instance of <see cref="KeyHandler"/>.
        /// </summary>
        public KeyHandler(IConsole console)
        {
            _console = console;

            _keyActions = new Dictionary<string, Action>
            {
                ["LeftArrow"] = () => MoveCursorLeft(),
                ["RightArrow"] = MoveCursorRight,

                ["Home"] = () =>
                {
                    while (!IsStartOfLine)
                        MoveCursorLeft();
                },
                ["End"] = () =>
                {
                    while (!IsEndOfLine)
                        MoveCursorRight();
                },
                ["Backspace"] = () => Backspace(),
                ["Delete"] = Delete,
                ["Insert"] = () => { }, // TODO: how to change to insertion/normal mode
                ["Escape"] = ClearLine,

                ["ControlLeftArrow"] = () => DoUntilPrevWordOrWhitespace(() => MoveCursorLeft()),
                ["ControlRightArrow"] = () => DoUntilNextWordOrWhitespace(MoveCursorRight),
                ["ControlBackspace"] = () => BackspacePrevWord(),
                ["ControlDelete"] = () => DoUntilPrevWordOrWhitespace(Delete)
            };
        }

        /// <summary>
        /// Initializes an instance of <see cref="KeyHandler"/>.
        /// </summary>
        public KeyHandler(IConsole console,
                          HashSet<ShortcutDefinition> internalShortcuts,
                          HashSet<ShortcutDefinition>? userDefinedShortcut = null) :
            this(console)
        {
            foreach (ShortcutDefinition shortcut in internalShortcuts)
            {
                string key = shortcut.ToString();
                if (!_keyActions.TryAdd(key, shortcut.Action))
                {
                    //Replace when already exists
                    _keyActions[shortcut.ToString()] = shortcut.Action;
                }
            }

            if (userDefinedShortcut != null)
            {
                foreach (ShortcutDefinition shortcut in userDefinedShortcut)
                {
                    if (!_keyActions.TryAdd(shortcut.ToString(), shortcut.Action))
                    {
                        //Throw an error when already exists
                        throw TypinException.DuplicatedShortcut(shortcut);
                    }
                }
            }
        }

        /// <summary>
        /// Handles key input.
        /// </summary>
        public void Handle(ConsoleKeyInfo keyInfo)
        {
            LastKey = keyInfo;

            if (_keyActions.TryGetValue(BuildKeyInput(keyInfo), out Action action))
            {
                action.Invoke();
            }
            else if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                     !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                Write('^');
                Write(keyInfo.Key.ToString());
            }
            else
                Write(keyInfo.KeyChar);
        }

        private string BuildKeyInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Modifiers == 0)
                return keyInfo.Key.ToString();

            return string.Concat(keyInfo.Modifiers.ToString(), keyInfo.Key.ToString());
        }

        /// <summary>
        /// Resets key handler to allow proper process of next line.
        /// </summary>
        public void Reset()
        {
            CursorPosition = 0;
            _text.Clear();
        }

        //TODO: rewrite: must work when line is wrapped
        private void MoveCursorLeft(int count = 1)
        {
            if (CursorPosition < count)
                count = CursorPosition;

            if (_console.CursorLeft < count)
                _console.SetCursorPosition(_console.BufferWidth - 1, _console.CursorTop - 1);
            else
                _console.SetCursorPosition(_console.CursorLeft - count, _console.CursorTop);

            CursorPosition -= count;
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine)
                return;

            if (IsEndOfBuffer)
                _console.SetCursorPosition(0, _console.CursorTop + 1);
            else
                _console.SetCursorPosition(_console.CursorLeft + 1, _console.CursorTop);

            ++CursorPosition;
        }

        public void ClearLine()
        {
            while (!IsStartOfLine)
                Backspace();

            _text.Clear();
        }

        public void Write(string str)
        {
            foreach (char character in str)
                Write(character);
        }

        public void Write(char c)
        {
            if (IsEndOfLine)
            {
                _text.Append(c);
                _console.Output.Write(c.ToString());
                CursorPosition++;
            }
            else
            {
                int left = _console.CursorLeft;
                int top = _console.CursorTop;
                string str = _text.ToString().Substring(CursorPosition);
                _text.Insert(CursorPosition, c);
                _console.Output.Write(c.ToString() + str);
                _console.SetCursorPosition(left, top);
                MoveCursorRight();
            }
        }
        public void Backspace(int count = 1)
        {
            for (; count > 0; --count)
            {
                if (CursorPosition == 0)
                    return;

                MoveCursorLeft(1);
                int index = CursorPosition;
                _text.Remove(index, 1);

                string replacement = _text.ToString().Substring(index);
                int left = _console.CursorLeft;
                int top = _console.CursorTop;

                string spaces = new string(' ', 1);
                _console.Output.Write(string.Format("{0}{1}", replacement, spaces));
                _console.SetCursorPosition(left, top);
            }
        }

        public void BackspacePrevWord()
        {
            DoUntilPrevWordOrWhitespace(() => Backspace());
        }

        private void Delete()
        {
            if (IsEndOfLine)
                return;

            int index = CursorPosition;
            _text.Remove(index, 1);

            string replacement = _text.ToString().Substring(index);
            int left = _console.CursorLeft;
            int top = _console.CursorTop;
            _console.Output.Write(string.Format("{0} ", replacement));
            _console.SetCursorPosition(left, top);
        }

        private void DoUntilPrevWordOrWhitespace(Action action)
        {
            int v = CursorPosition - 1;
            if (v < 0)
                return;

            if (char.IsWhiteSpace(_text[v]))
            {
                do
                {
                    action();
                }
                while (!IsStartOfLine && char.IsWhiteSpace(_text[CursorPosition - 1]));

                return;
            }

            do
            {
                action();
            }
            while (!IsStartOfLine && !char.IsWhiteSpace(_text[CursorPosition - 1]));
        }

        private void DoUntilNextWordOrWhitespace(Action action)
        {
            if (IsEndOfLine)
                return;

            if (char.IsWhiteSpace(_text[CursorPosition]))
            {
                do
                {
                    action();
                }
                while (!IsEndOfLine && char.IsWhiteSpace(_text[CursorPosition]));

                return;
            }

            do
            {
                action();
            }
            while (!IsEndOfLine && !char.IsWhiteSpace(_text[CursorPosition]));
        }
    }
}
