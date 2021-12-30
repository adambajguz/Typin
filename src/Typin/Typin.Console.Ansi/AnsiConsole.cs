namespace Typin.Console
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.IO;

    /// <summary>
    /// ANSI/VT100 compatible console.
    /// </summary>
    public sealed class AnsiConsole : IConsole
    {
        private ConsoleColor _foregroundColor = ConsoleColor.White;
        private ConsoleColor _backgroundColor = ConsoleColor.Black;

        private int _cursorLeft;
        private int _cursorTop;

        /// <inheritdoc/>
        public StandardStreamReader Input { get; }

        /// <inheritdoc/>
        public StandardStreamWriter Output { get; }

        /// <inheritdoc/>
        public StandardStreamWriter Error { get; }

        /// <inheritdoc/>
        public ConsoleFeatures Features { get; } = ConsoleFeatures.All;

        /// <inheritdoc/>
        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.White : value;

                Output.Write(Ansi.Color.Foreground.FromConsoleColor(value));
            }
        }

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.Black : value;

                Output.Write(Ansi.Color.Background.FromConsoleColor(value));
            }
        }

        /// <inheritdoc/>
        public int CursorLeft
        {
            get => _cursorLeft;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CursorTop
        {
            get => _cursorTop;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int WindowWidth
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int WindowHeight
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int BufferWidth
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int BufferHeight
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AnsiConsole"/>.
        /// </summary>
        public AnsiConsole(Stream? input = null, bool isInputRedirected = true,
                           Stream? output = null, bool isOutputRedirected = true,
                           Stream? error = null, bool isErrorRedirected = true)
        {
            Input = WrapInput(this, input, isInputRedirected);
            Output = WrapOutput(this, output, isOutputRedirected);
            Error = WrapOutput(this, error, isErrorRedirected);
        }

        /// <inheritdoc/>
        public void Clear()
        {

        }

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ResetColor()
        {
            _foregroundColor = ConsoleColor.White;
            _backgroundColor = ConsoleColor.Black;

            Output.Write(string.Concat(Ansi.Color.Background.Default, Ansi.Color.Foreground.Default));
        }

        /// <inheritdoc />
        public void SetBackground(byte r, byte g, byte b)
        {
            Output.Write(Ansi.Color.Background.Rgb(r, g, b));
        }

        /// <inheritdoc />
        public void SetForeground(byte r, byte g, byte b)
        {
            Output.Write(Ansi.Color.Foreground.Rgb(r, g, b));
        }

        /// <inheritdoc />
        public void SetColors(byte br, byte bg, byte bb,
                              byte fr, byte fg, byte fb)
        {
            Output.Write(string.Concat(Ansi.Color.Background.Rgb(br, bg, bb), Ansi.Color.Foreground.Rgb(fr, fg, fb)));
        }

        /// <inheritdoc/>
        public void SetCursorPosition(int left, int top)
        {
            _cursorLeft = left;
            _cursorTop = top;

            Output.Write(Ansi.Cursor.Move.ToLocation(left, top));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Error.Dispose();
        }

        /// <inheritdoc/>
        public Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #region Helpers
        private static StandardStreamReader WrapInput(IConsole console, Stream? stream, bool isRedirected)
        {
            if (stream is null)
            {
                return StandardStreamReader.CreateNull(console);
            }

            return new StandardStreamReader(Stream.Synchronized(stream), Console.InputEncoding, false, isRedirected, console);
        }

        private static StandardStreamWriter WrapOutput(IConsole console, Stream? stream, bool isRedirected)
        {
            if (stream is null)
            {
                return StandardStreamWriter.CreateNull(console);
            }

            return new StandardStreamWriter(Stream.Synchronized(stream), Console.OutputEncoding, isRedirected, console)
            {
                AutoFlush = true
            };
        }
        #endregion
    }
}
