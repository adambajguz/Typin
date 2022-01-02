namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.Extensions;
    using Typin.Console.IO;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that routes all data to preconfigured streams.
    /// Does not leak to system console in any way.
    /// Use this class as a substitute for system console when running tests.
    /// </summary>
    public class VirtualConsole : IConsole, IDisposable
    {
        private bool _disposedValue;

        /// <inheritdoc />
        public StandardStreamReader Input { get; }

        /// <inheritdoc />
        public StandardStreamWriter Output { get; }

        /// <inheritdoc />
        public StandardStreamWriter Error { get; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        /// <inheritdoc />
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

        /// <inheritdoc />
        public ConsoleFeatures Features { get; } = ConsoleFeatures.ReadKey;

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="VirtualConsole"/>.
        /// Use named parameters to specify the streams you want to override.
        /// </summary>
        public VirtualConsole(Stream? input = null, bool isInputRedirected = true,
                              Stream? output = null, bool isOutputRedirected = true,
                              Stream? error = null, bool isErrorRedirected = true)
        {
            Input = WrapInput(this, input, isInputRedirected);
            Output = WrapOutput(this, output, isOutputRedirected);
            Error = WrapOutput(this, error, isErrorRedirected);
        }

        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualConsole console, MemoryStreamWriter output, MemoryStreamWriter error) CreateBuffered(bool isInputRedirected = true,
                                                                                                                   bool isOutputRedirected = true,
                                                                                                                   bool isErrorRedirected = true)
        {
            // Memory streams don't need to be disposed
            MemoryStreamWriter output = new(Console.OutputEncoding);
            MemoryStreamWriter error = new(Console.OutputEncoding);

            VirtualConsole console = new(input: null, isInputRedirected,
                                         output.Stream, isOutputRedirected,
                                         error.Stream, isErrorRedirected);

            return (console, output, error);
        }


        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualConsole console, MemoryStreamReader input, MemoryStreamWriter output, MemoryStreamWriter error) CreateBufferedWithInput(bool isInputRedirected = true,
                                                                                                                                                      bool isOutputRedirected = true,
                                                                                                                                                      bool isErrorRedirected = true)
        {
            // Memory streams don't need to be disposed
            MemoryStreamReader input = new(Console.InputEncoding);
            MemoryStreamWriter output = new(Console.OutputEncoding);
            MemoryStreamWriter error = new(Console.OutputEncoding);

            VirtualConsole console = new(input.Stream, isInputRedirected,
                                         output.Stream, isOutputRedirected,
                                         error.Stream, isErrorRedirected);

            return (console, input, output, error);
        }
        #endregion

        /// <inheritdoc />
        public void Clear()
        {

        }

        /// <inheritdoc />
        public void ResetColor()
        {
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }

        /// <inheritdoc />
        public void SetBackground(byte r, byte g, byte b)
        {

        }

        /// <inheritdoc />
        public void SetForeground(byte r, byte g, byte b)
        {

        }


        /// <inheritdoc />
        public void SetColors(byte br, byte bg, byte bb,
                              byte fr, byte fg, byte fb)
        {

        }

        /// <inheritdoc />
        public int CursorLeft { get; set; }

        /// <inheritdoc />
        public int CursorTop { get; set; }

        /// <inheritdoc />
        public int WindowWidth { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int WindowHeight { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int BufferWidth { get; set; } = int.MaxValue;

        /// <inheritdoc />
        public int BufferHeight { get; set; } = int.MaxValue;

        /// <inheritdoc/>
        public void SetCursorPosition(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            int v = -1;
            while (v < 0)
            {
                v = Input.Read();
            }

            Output.Write((char)v);

            //TODO: fix double enter for \r\

            return ((char)v).ToConsoleKeyInfo();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            char[] charsRead = new char[1];

            int v = -1;
            while (v < 0)
            {
                v = await Input.ReadAsync(charsRead, cancellationToken);
            }

            Output.Write(charsRead[0]);

            //TODO: fix double enter for \r\

            return charsRead[0].ToConsoleKeyInfo();
        }

        /// <summary>
        /// Disposes console.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Input.Dispose();
                    Output.Dispose();
                    Error.Dispose();
                }

                _disposedValue = true;
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