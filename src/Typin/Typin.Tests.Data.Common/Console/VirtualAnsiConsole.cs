namespace Typin.Tests.Data.Console
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Console.IO;

    public class VirtualAnsiConsole : IConsole, IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private bool disposedValue;

        private int _cursorLeft;
        private int _cursorTop;
        private ConsoleColor _foregroundColor = ConsoleColor.White;
        private ConsoleColor _backgroundColor = ConsoleColor.Black;

        /// <inheritdoc />
        public StandardStreamReader Input { get; }

        /// <inheritdoc />
        public StandardStreamWriter Output { get; }

        /// <inheritdoc />
        public StandardStreamWriter Error { get; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;

            //https://misc.flogisoft.com/bash/tip_colors_and_formatting
            set
            {
                _foregroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.White : value;

                Output.Write(Ansi.Color.Foreground.FromConsoleColor(value));
            }
        }

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.Black : value;

                Output.Write(Ansi.Color.Background.FromConsoleColor(value));
            }
        }

        /// <inheritdoc />
        public int CursorLeft
        {
            get => _cursorLeft;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int CursorTop
        {
            get => _cursorTop;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int WindowWidth { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int WindowHeight { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int BufferWidth { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int BufferHeight { get; set; } = int.MaxValue;

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="VirtualConsole"/>.
        /// Use named parameters to specify the streams you want to override.
        /// </summary>
        public VirtualAnsiConsole(Stream? input = null, bool isInputRedirected = true,
                                  Stream? output = null, bool isOutputRedirected = true,
                                  Stream? error = null, bool isErrorRedirected = true,
                                  CancellationToken cancellationToken = default)
        {
            Input = WrapInput(this, input, isInputRedirected);
            Output = WrapOutput(this, output, isOutputRedirected);
            Error = WrapOutput(this, error, isErrorRedirected);

            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualAnsiConsole console, MemoryStreamWriter output, MemoryStreamWriter error) CreateBuffered(bool isInputRedirected = true,
                                                                                                                       bool isOutputRedirected = true,
                                                                                                                       bool isErrorRedirected = true,
                                                                                                                       CancellationToken cancellationToken = default)
        {
            // Memory streams don't need to be disposed
            MemoryStreamWriter output = new(Console.OutputEncoding);
            MemoryStreamWriter error = new(Console.OutputEncoding);

            VirtualAnsiConsole console = new(input: null, isInputRedirected,
                                             output.Stream, isOutputRedirected,
                                             error.Stream, isErrorRedirected,
                                             cancellationToken);

            return (console, output, error);
        }
        #endregion

        public void Clear()
        {

        }

        public CancellationToken GetCancellationToken()
        {
            return _cancellationToken;
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            throw new NotImplementedException();
        }

        public Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void ResetColor()
        {
            _foregroundColor = ConsoleColor.White;
            _backgroundColor = ConsoleColor.Black;

            Output.Write(string.Concat(Ansi.Color.Foreground.Default, Ansi.Color.Background.Default));
        }

        public void SetCursorPosition(int left, int top)
        {
            _cursorLeft = left;
            _cursorTop = top;

            Output.Write(Ansi.Cursor.Move.ToLocation(left, top));
        }

        /// <summary>
        /// Disposes console.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Input.Dispose();
                    Output.Dispose();
                    Error.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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
