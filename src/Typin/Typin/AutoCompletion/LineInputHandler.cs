namespace Typin.AutoCompletion
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Internal.Exceptions;
    using static Typin.AutoCompletion.KeyHandler;

    internal sealed class LineInputHandler
    {
        private readonly IConsole _console;
        private readonly KeyHandler _keyHandler;
        private readonly HashSet<ShortcutDefinition> _shortcuts;
        private readonly StringBuilder _text = new();

        public event InputModifiedEventHandler? InputModified
        {
            add
            {
                _keyHandler.InputModified += value;
            }
            remove
            {
                _keyHandler.InputModified -= value;
            }
        }

        /// <summary>
        /// Cursor position relative to input.
        /// </summary>
        public int CursorPosition { get; private set; }

        /// <summary>
        /// Whether input is at the start of line.
        /// </summary>
        public bool IsStartOfLine => CursorPosition == 0;

        /// <summary>
        /// Whether input is at the end of line.
        /// </summary>
        public bool IsEndOfLine => CursorPosition == _text.Length;

        /// <summary>
        /// Whether input is at the start of console buffer.
        /// </summary>
        public bool IsEndOfBuffer => _console.CursorLeft == _console.BufferWidth - 1;

        /// <summary>
        /// Whether text is being read from console (false line reading is finished).
        /// </summary>
        public bool IsReading { get; private set; }

        /// <summary>
        /// Current input text.
        /// </summary>
        public string CurrentInput => _text.ToString().TrimEnd('\n', '\r');

        /// <summary>
        /// Last input text.
        /// </summary>
        public string LastInput { get; private set; } = string.Empty;

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="LineInputHandler"/>.
        /// </summary>
        private LineInputHandler(IConsole console)
        {
            _console = console;

            _shortcuts = new HashSet<ShortcutDefinition>
            {
                new ShortcutDefinition(ConsoleKey.LeftArrow, () => MoveCursorLeft()),
                new ShortcutDefinition(ConsoleKey.RightArrow, MoveCursorRight),
                new ShortcutDefinition(ConsoleKey.Home, () =>
                {
                    while (!IsStartOfLine) { MoveCursorLeft(); } }),
                new ShortcutDefinition(ConsoleKey.End, () =>
                {
                    while (!IsEndOfLine) { MoveCursorRight(); } }),
                new ShortcutDefinition(ConsoleKey.Backspace, true, () => Backspace()),
                new ShortcutDefinition(ConsoleKey.Delete, true, Delete),
                new ShortcutDefinition(ConsoleKey.Insert, () => { }),
                new ShortcutDefinition(ConsoleKey.Escape, true, ClearLine),

                new ShortcutDefinition(ConsoleKey.LeftArrow, ConsoleModifiers.Control, true, () => DoUntilPrevWordOrWhitespace(() => MoveCursorLeft())),
                new ShortcutDefinition(ConsoleKey.RightArrow, ConsoleModifiers.Control, true, () => DoUntilNextWordOrWhitespace(MoveCursorRight)),
                new ShortcutDefinition(ConsoleKey.Backspace, ConsoleModifiers.Control, true, BackspacePrevWord),
                new ShortcutDefinition(ConsoleKey.Delete, ConsoleModifiers.Control, true, DeleteNextWord)
            };

            _keyHandler = new KeyHandler(console, _shortcuts);
            _keyHandler.UnhandledControlSequenceDetected += ControlSequenceDetected;
            _keyHandler.UnhandledKeyDetected += UnhandledKeyDetected;
            _keyHandler.NewLineDetected += NewLineDetected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="LineInputHandler"/>.
        /// </summary>
        public LineInputHandler(IConsole console,
                                HashSet<ShortcutDefinition>? internalShortcuts = null,
                                HashSet<ShortcutDefinition>? userDefinedShortcuts = null) :
            this(console)
        {
            if (internalShortcuts is not null)
            {
                //TODO: maybe hashset is not the best collection
                //_shortcuts.Union(internalShortcuts);
                foreach (ShortcutDefinition shortcut in internalShortcuts)
                {
                    if (!_shortcuts.Add(shortcut))
                    {
                        //Replace when already exists
                        _shortcuts.Remove(shortcut);
                        _shortcuts.Add(shortcut);
                    }
                }
            }

            if (userDefinedShortcuts is not null)
            {
                //_shortcuts.Union(userDefinedShortcut);
                foreach (ShortcutDefinition shortcut in userDefinedShortcuts)
                {
                    if (!_shortcuts.Add(shortcut))
                    {
                        throw ModeEndUserExceptions.DuplicatedShortcut(shortcut);
                    }
                }
            }
        }
        #endregion

        #region KeyHandler callbacks
        private void NewLineDetected()
        {
            LastInput = _text.ToString().TrimEnd('\n', '\r');
            _text.Clear();

            IsReading = false;

            //Reset key handler to allow proper process of next line.
            _console.Output.WriteLine();
            CursorPosition = 0;
        }

        private void UnhandledKeyDetected(ref ConsoleKeyInfo keyInfo)
        {
            Write(keyInfo.KeyChar);
        }

        private void ControlSequenceDetected(ref ConsoleKeyInfo keyInfo)
        {
            Write('^');
            Write(keyInfo.Key.ToString());
        }
        #endregion

        #region ReadLine
        /// <summary>
        /// Reads a line from input.
        /// </summary>
        public async Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            IsReading = true;
            do
            {
                await _keyHandler.ReadKeyAsync(cancellationToken);
            } while (IsReading && !cancellationToken.IsCancellationRequested);

            return LastInput;
        }

        /// <summary>
        /// Reads a sequence from array. When sequence is terminated with Enter, acts as ReadLine.
        /// </summary>
        public async Task<string> ReadAsync(params ConsoleKeyInfo[] line)
        {
            IsReading = true;

            for (int i = 0; i < line.Length && IsReading; ++i)
            {
                ConsoleKeyInfo keyInfo = line[i];
                await _keyHandler.ReadKeyAsync(keyInfo);
            }

            return IsReading ? CurrentInput : LastInput;
        }
        #endregion

        #region Write
        public void Write(string str)
        {
            foreach (char character in str)
            {
                Write(character);
            }
        }

        public void Write(char c)
        {
            if (c == '\0')
            {
                return;
            }

            if (IsEndOfLine)
            {
                _text.Append(c);
                _console.Output.Write(c);
                ++CursorPosition;
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
        #endregion

        #region MoveCursor
        //TODO: rewrite: must work when line is wrapped
        private void MoveCursorLeft(int count = 1)
        {
            if (CursorPosition < count)
            {
                count = CursorPosition;
            }

            if (_console.CursorLeft < count)
            {
                _console.SetCursorPosition(_console.BufferWidth - 1, _console.CursorTop - 1);
            }
            else
            {
                _console.SetCursorPosition(_console.CursorLeft - count, _console.CursorTop);
            }

            CursorPosition -= count;
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine)
            {
                return;
            }

            if (IsEndOfBuffer)
            {
                _console.SetCursorPosition(0, _console.CursorTop + 1);
            }
            else
            {
                _console.SetCursorPosition(_console.CursorLeft + 1, _console.CursorTop);
            }

            ++CursorPosition;
        }
        #endregion

        #region Input text deletion
        public void ClearLine()
        {
            while (!IsStartOfLine)
            {
                Backspace();
            }

            _text.Clear();
        }

        public void Backspace(int count = 1)
        {
            for (; count > 0; --count)
            {
                if (CursorPosition == 0)
                {
                    return;
                }

                MoveCursorLeft(1);
                int index = CursorPosition;
                _text.Remove(index, 1);

                string replacement = _text.ToString().Substring(index);
                int left = _console.CursorLeft;
                int top = _console.CursorTop;

                string spaces = new(' ', 1);
                _console.Output.Write(string.Format("{0}{1}", replacement, spaces));
                _console.SetCursorPosition(left, top);
            }
        }

        public void BackspacePrevWord()
        {
            DoUntilPrevWordOrWhitespace(() => Backspace());
        }

        public void DeleteNextWord()
        {
            //TODO: Use Delete instead of Backspace or maybe remove delete?
            if (IsEndOfLine)
            {
                return;
            }

            DoUntilNextWordOrWhitespace(MoveCursorRight);
            DoUntilPrevWordOrWhitespace(() => Backspace());
        }

        private void Delete()
        {
            if (IsEndOfLine)
            {
                return;
            }

            int index = CursorPosition;
            _text.Remove(index, 1);

            string replacement = _text.ToString().Substring(index);
            int left = _console.CursorLeft;
            int top = _console.CursorTop;

            _console.Output.Write(string.Format("{0} ", replacement));
            _console.SetCursorPosition(left, top);
        }
        #endregion

        #region DoUntil
        private void DoUntilPrevWordOrWhitespace(Action action, bool treatSpacesAsWord = false)
        {
            if (IsStartOfLine)
            {
                return;
            }

            if (char.IsWhiteSpace(_text[CursorPosition - 1]))
            {
                do
                {
                    action();
                }
                while (!IsStartOfLine && char.IsWhiteSpace(_text[CursorPosition - 1]));

                if (treatSpacesAsWord)
                {
                    return;
                }
            }

            do
            {
                action();
            }
            while (!IsStartOfLine && !char.IsWhiteSpace(_text[CursorPosition - 1]));
        }

        private void DoUntilNextWordOrWhitespace(Action action, bool treatSpacesAsWord = false)
        {
            if (IsEndOfLine)
            {
                return;
            }

            if (char.IsWhiteSpace(_text[CursorPosition + 1]))
            {
                do
                {
                    action();
                }
                while (!IsEndOfLine && char.IsWhiteSpace(_text[CursorPosition + 1]));

                if (treatSpacesAsWord)
                {
                    return;
                }
            }

            //int v = treatSpacesAsWord ? 0 : 1;
            do
            {
                action();
            }
            while (!IsEndOfLine && (char.IsWhiteSpace(_text[CursorPosition]) || !char.IsWhiteSpace(_text[CursorPosition - 1])));
        }
        #endregion
    }
}
