namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.IO;
    using Typin.Extensions;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that wraps the default system console.
    /// </summary>
    public class SystemConsole : IConsole
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private bool disposedValue;

        /// <inheritdoc />
        public StandardStreamReader Input { get; }

        /// <inheritdoc />
        public StandardStreamWriter Output { get; }

        /// <inheritdoc />
        public StandardStreamWriter Error { get; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="SystemConsole"/>.
        /// </summary>
        public SystemConsole()
        {
            Input = WrapInput(this, Console.OpenStandardInput(), Console.IsInputRedirected);
            Output = WrapOutput(this, Console.OpenStandardOutput(), Console.IsOutputRedirected);
            Error = WrapOutput(this, Console.OpenStandardError(), Console.IsErrorRedirected);
        }
        #endregion

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void ResetColor()
        {
            Console.ResetColor();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <inheritdoc />
        public CancellationToken GetCancellationToken()
        {
            if (_cancellationTokenSource is not null)
            {
                return _cancellationTokenSource.Token;
            }

            /* ====================================================================================================================
             *
             *  This methods must use local variable, because removing cts and using _cancellationTokenSource
             *  would lead to very high RAM usage when many CliApplication were created in single process e.g. in benchmarks.
             *
             *  (Memory leak? - this needs further investigation)
             *
             * ====================================================================================================================
             *
             *   public CancellationToken GetCancellationToken()
             *   {
             *       if (_cancellationTokenSource is not null)
             *           return _cancellationTokenSource.Token;
             *
             *       _cancellationTokenSource = new CancellationTokenSource();
             *
             *       Console.CancelKeyPress += (_, args) =>
             *       {
             *           // If cancellation hasn't been requested yet - cancel shutdown and fire the token
             *           if (!_cancellationTokenSource.IsCancellationRequested)
             *           {
             *               args.Cancel = true;
             *               _cancellationTokenSource.Cancel();
             *           }
             *       };
             *
             *       return _cancellationTokenSource.Token;
             *   }
             * ====================================================================================================================
             */

            CancellationTokenSource cts = new();

            Console.CancelKeyPress += (_, args) =>
            {
                //TODO: remove token from console + add logging + add supprot for cancelling single command in interactive mode.
                // If cancellation hasn't been requested yet - cancel shutdown and fire the token
                if (!cts.IsCancellationRequested)
                {
                    args.Cancel = true;
                    cts.Cancel();
                }
            };

            return (_cancellationTokenSource = cts).Token;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            //TODO: fix double enter for \r\
            if (Input.IsRedirected)
            {
                int v = -1;
                while (v < 0)
                {
                    v = Input.Read();
                }

                Output.Write((char)v);

                return ((char)v).ToConsoleKeyInfo();
            }

            return Console.ReadKey(intercept);
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            CancellationToken cancellationToken = GetCancellationToken();

            char[] charsRead = new char[1];

            //TODO: fix double enter for \r\
            if (Input.IsRedirected)
            {
                int v = -1;
                while (v < 0 && !cancellationToken.IsCancellationRequested)
                {
#if NETSTANDARD2_0
                    v = await Task.Run(() => Input.ReadAsync(charsRead, 0, 1), cancellationToken);
#else
                    v = await Input.ReadAsync(charsRead, cancellationToken);
#endif
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException($"{nameof(ReadKeyAsync)} cancelled.");
                }

                Output.Write(charsRead[0]);

                return (charsRead[0]).ToConsoleKeyInfo();
            }

            while (!Console.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException($"{nameof(ReadKeyAsync)} cancelled.");
            }

            return Console.ReadKey(intercept);
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