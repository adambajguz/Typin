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
    /// Implementation of <see cref="IConsole"/> that wraps the default system console.
    /// </summary>
    public sealed class SystemConsole : IConsole
    {
        /// <inheritdoc />
        public StandardStreamReader Input { get; }

        /// <inheritdoc />
        public StandardStreamWriter Output { get; }

        /// <inheritdoc />
        public StandardStreamWriter Error { get; }

        /// <inheritdoc/>
        public ConsoleFeatures Features { get; } =
            ConsoleFeatures.ConsoleColors |
            ConsoleFeatures.Clear |
            ConsoleFeatures.CursorPosition |
            ConsoleFeatures.WindowDimensions |
            ConsoleFeatures.BufferDimensions |
            ConsoleFeatures.ReadKey;

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
        public void SetBackground(byte r, byte g, byte b)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void SetForeground(byte r, byte g, byte b)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void SetColors(byte br, byte bg, byte bb,
                              byte fr, byte fg, byte fb)
        {
            throw new NotSupportedException();
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
        public async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            char[] charsRead = new char[1];

            //TODO: fix double enter for \r\
            if (Input.IsRedirected)
            {
                int v = -1;
                while (v < 0 && !cancellationToken.IsCancellationRequested)
                {
                    v = await Input.ReadAsync(charsRead, cancellationToken);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException($"{nameof(ReadKeyAsync)} cancelled.");
                }

                Output.Write(charsRead[0]);

                return charsRead[0].ToConsoleKeyInfo();
            }

            while (!Console.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException($"{nameof(ReadKeyAsync)} cancelled.");
            }

            return Console.ReadKey(intercept);
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