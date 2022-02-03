namespace Typin.Console.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="StreamReader"/> wrapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StandardStreamReader : StreamReader, IStandardRedirectableConsoleStream, IKeyReader
    {
        /// <summary>
        /// A <see cref="StandardStreamReader"/> object around an empty stream.
        /// </summary>
        public static StandardStreamReader CreateNull(IConsole boundedConsole)
        {
            return new StandardStreamReader(Stream.Null, new UTF8Encoding(false, true), isRedirected: true, boundedConsole);
        }

        /// <inheritdoc/>
        public IConsole BoundedConsole { get; }

        /// <summary>
        /// Optional key reader provider. When null, key reading is not supported.
        /// </summary>
        public IKeyReader? KeyReader { get; }

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    bool detectEncodingFromByteOrderMarks,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    Encoding encoding,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    Encoding encoding,
                                    bool detectEncodingFromByteOrderMarks,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    Encoding encoding,
                                    bool detectEncodingFromByteOrderMarks,
                                    int bufferSize,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream,
                                    Encoding encoding,
                                    bool detectEncodingFromByteOrderMarks,
                                    int bufferSize,
                                    bool leaveOpen,
                                    bool isRedirected,
                                    IConsole boundedConsole,
                                    IKeyReader? keyReader = null) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
            KeyReader = keyReader;
        }

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return KeyReader?.ReadKey(intercept) ??
                throw new NotSupportedException($"{nameof(KeyReader)} is not supported by {typeof(StandardStreamReader)} bounded with {BoundedConsole.GetType()} console.");
        }

        /// <inheritdoc/>
        public async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            if (KeyReader is null)
            {
                throw new NotSupportedException($"{nameof(KeyReader)} is not supported by {typeof(StandardStreamReader)} bounded with {BoundedConsole.GetType()} console.");
            }

            return await KeyReader.ReadKeyAsync(intercept, cancellationToken);
        }

        // The overrides below are required to establish thread-safe behavior
        // in methods deriving from StreamReader.

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override int Peek()
        {
            return base.Peek();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override int Read()
        {
            return base.Read();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return base.ReadBlock(buffer, index, count);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override string? ReadLine()
        {
            return base.ReadLine();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override string ReadToEnd()
        {
            return base.ReadToEnd();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            // Must be non-async to work with locks
            return Task.FromResult(Read(buffer, index, count));
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            // Must be non-async to work with locks
            return Task.FromResult(ReadBlock(buffer, index, count));
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task<string?> ReadLineAsync()
        {
            // Must be non-async to work with locks
            return Task.FromResult(ReadLine());
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task<string> ReadToEndAsync()
        {
            // Must be non-async to work with locks
            return Task.FromResult(ReadToEnd());
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Close()
        {
            base.Close();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
