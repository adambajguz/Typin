namespace Typin.Console.IO
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="StreamWriter"/> wrapper.
    /// </summary>
    public class StandardStreamWriter : StreamWriter, IStandardRedirectableConsoleStream
    {
        /// <summary>
        /// A <see cref="StandardStreamWriter"/> object around an empty stream.
        /// </summary>
        public static new readonly StandardStreamWriter Null = new StandardStreamWriter(Stream.Null, new UTF8Encoding(false, true), 128, true, isRedirected: true, bounedConsole: null);

        /// <inheritdoc/>
        public IConsole BoundedConsole { get; }

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, bool isRedirected, IConsole bounedConsole) :
            base(stream)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding, bufferSize)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }
    }
}
