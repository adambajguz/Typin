namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;
    using Typin.Extensions;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that wraps the default system console.
    /// </summary>
    public class SystemConsole : IConsole
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private bool disposedValue;

        /// <inheritdoc />
        public StreamReader Input { get; }

        /// <inheritdoc />
        public bool IsInputRedirected => Console.IsInputRedirected;

        /// <inheritdoc />
        public StreamWriter Output { get; }

        /// <inheritdoc />
        public bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <inheritdoc />
        public StreamWriter Error { get; }

        /// <inheritdoc />
        public bool IsErrorRedirected => Console.IsErrorRedirected;

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
            Input = WrapInput(Console.OpenStandardInput());
            Output = WrapOutput(Console.OpenStandardOutput());
            Error = WrapOutput(Console.OpenStandardError());
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
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <inheritdoc />
        public CancellationToken GetCancellationToken()
        {
            if (_cancellationTokenSource != null)
                return _cancellationTokenSource.Token;

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
             *       if (_cancellationTokenSource != null)
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

            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, args) =>
            {
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
            if (IsInputRedirected)
                return ((char)Input.Read()).ToConsoleKeyInfo();

            return Console.ReadKey(intercept);
            //return Task.Run(() => Console.ReadKey(intercept)).Result;
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
        #endregion
    }
}