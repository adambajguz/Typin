namespace Typin.Console.IO
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="StreamWriter"/> wrapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StandardStreamWriter : StreamWriter, IStandardRedirectableConsoleStream
    {
        /// <summary>
        /// Returns a <see cref="StandardStreamWriter"/> object around an empty stream.
        /// </summary>
        public static StandardStreamWriter CreateNull(IConsole boundedConsole)
        {
            return new StandardStreamWriter(Stream.Null, new UTF8Encoding(false, true), 128, true, isRedirected: true, boundedConsole);
        }

        /// <inheritdoc/>
        public IConsole BoundedConsole { get; }

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream,
                                    bool isRedirected,
                                    IConsole boundedConsole) :
            base(stream)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream,
                                    Encoding encoding,
                                    bool isRedirected,
                                    IConsole boundedConsole) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream,
                                    Encoding encoding,
                                    int bufferSize,
                                    bool isRedirected,
                                    IConsole boundedConsole) :
            base(stream, encoding, bufferSize)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream,
                                    Encoding encoding,
                                    int bufferSize,
                                    bool leaveOpen,
                                    bool isRedirected,
                                    IConsole boundedConsole) :
            base(stream, encoding, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        // The overrides below are required to establish thread-safe behavior
        // in methods deriving from StreamWriter.

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(char[]? buffer)
        {
            base.Write(buffer);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(char value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string? value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string format, object? arg0)
        {
            base.Write(format, arg0);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string format, object? arg0, object? arg1)
        {
            base.Write(format, arg0, arg1);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string format, object? arg0, object? arg1, object? arg2)
        {
            base.Write(format, arg0, arg1, arg2);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string format, params object?[] arg)
        {
            base.Write(format, arg);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(bool value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(int value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(long value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(uint value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(ulong value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(float value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(double value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(decimal value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(object? value)
        {
            base.Write(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            // Must be non-async to work with locks
            Write(buffer, index, count);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteAsync(char value)
        {
            // Must be non-async to work with locks
            Write(value);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteAsync(string? value)
        {
            // Must be non-async to work with locks
            Write(value);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine()
        {
            base.WriteLine();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(char[] buffer, int index, int count)
        {
            base.WriteLine(buffer, index, count);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(char[]? buffer)
        {
            base.WriteLine(buffer);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(char value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string? value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string format, object? arg0)
        {
            base.WriteLine(format, arg0);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string format, object? arg0, object? arg1)
        {
            base.WriteLine(format, arg0, arg1);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string format, object? arg0, object? arg1, object? arg2)
        {
            base.WriteLine(format, arg0, arg1, arg2);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string format, params object?[] arg)
        {
            base.WriteLine(format, arg);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(bool value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(int value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(long value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(uint value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(ulong value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(float value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(double value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(decimal value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(object? value)
        {
            base.WriteLine(value);
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteLineAsync()
        {
            // Must be non-async to work with locks
            WriteLine();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteLineAsync(char value)
        {
            // Must be non-async to work with locks
            WriteLine(value);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            // Must be non-async to work with locks
            WriteLine(buffer, index, count);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task WriteLineAsync(string? value)
        {
            // Must be non-async to work with locks
            WriteLine(value);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override void Flush()
        {
            base.Flush();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage, MethodImpl(MethodImplOptions.Synchronized)]
        public override Task FlushAsync()
        {
            // Must be non-async to work with locks
            Flush();
            return Task.CompletedTask;
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
