namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;
    using Typin.Extensions;
    using Typin.Utilities.CliFx.Utilities;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that routes all data to preconfigured streams.
    /// Does not leak to system console in any way.
    /// Use this class as a substitute for system console when running tests.
    /// </summary>
    public partial class VirtualConsole : IConsole
    {
        private readonly CancellationToken _cancellationToken;
        private bool disposedValue;

        /// <inheritdoc />
        public StreamReader Input { get; }

        /// <inheritdoc />
        public bool IsInputRedirected { get; }

        /// <inheritdoc />
        public StreamWriter Output { get; }

        /// <inheritdoc />
        public bool IsOutputRedirected { get; }

        /// <inheritdoc />
        public StreamWriter Error { get; }

        /// <inheritdoc />
        public bool IsErrorRedirected { get; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        /// <inheritdoc />
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Initializes an instance of <see cref="VirtualConsole"/>.
        /// Use named parameters to specify the streams you want to override.
        /// </summary>
        public VirtualConsole(StreamReader? input = null, bool isInputRedirected = true,
                              StreamWriter? output = null, bool isOutputRedirected = true,
                              StreamWriter? error = null, bool isErrorRedirected = true,
                              CancellationToken cancellationToken = default)
        {
            Input = input ?? StreamReader.Null;
            IsInputRedirected = isInputRedirected;

            Output = output ?? StreamWriter.Null;
            IsOutputRedirected = isOutputRedirected;

            Error = error ?? StreamWriter.Null;
            IsErrorRedirected = isErrorRedirected;

            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Initializes an instance of <see cref="VirtualConsole"/>.
        /// Use named parameters to specify the streams you want to override.
        /// </summary>
        public VirtualConsole(Stream? input = null, bool isInputRedirected = true,
                              Stream? output = null, bool isOutputRedirected = true,
                              Stream? error = null, bool isErrorRedirected = true,
                              CancellationToken cancellationToken = default)
            : this(WrapInput(input), isInputRedirected,
                   WrapOutput(output), isOutputRedirected,
                   WrapOutput(error), isErrorRedirected,
                   cancellationToken)
        {

        }

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

        /// <inheritdoc />
        public CancellationToken GetCancellationToken()
        {
            return _cancellationToken;
        }

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
            return ((char)Input.Read()).ToConsoleKeyInfo();
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
    }

    public partial class VirtualConsole
    {
        private static StreamReader WrapInput(Stream? stream)
        {
            if (stream is null)
                return StreamReader.Null;

            return new StreamReader(Stream.Synchronized(stream), Console.InputEncoding, false);
        }

        private static StreamWriter WrapOutput(Stream? stream)
        {
            if (stream is null)
                return StreamWriter.Null;

            return new StreamWriter(Stream.Synchronized(stream), Console.OutputEncoding)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualConsole console, MemoryStreamWriter output, MemoryStreamWriter error) CreateBuffered(bool isOutputRedirected = true,
                                                                                                                   bool isErrorRedirected = true,
                                                                                                                   CancellationToken cancellationToken = default)
        {
            // Memory streams don't need to be disposed
            var output = new MemoryStreamWriter(Console.OutputEncoding);
            var error = new MemoryStreamWriter(Console.OutputEncoding);

            var console = new VirtualConsole(output: output, isOutputRedirected: isOutputRedirected,
                                             error: error, isErrorRedirected: isErrorRedirected,
                                             cancellationToken: cancellationToken);

            return (console, output, error);
        }
    }
}