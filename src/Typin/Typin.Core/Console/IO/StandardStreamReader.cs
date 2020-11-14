namespace Typin.Console.IO
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="StreamReader"/> wrapper.
    /// </summary>
    public class StandardStreamReader : StreamReader, IStandardRedirectableConsoleStream
    {
        /// <summary>
        /// A <see cref="StandardStreamReader"/> object around an empty stream.
        /// </summary>
        public static new readonly StandardStreamReader Null = new StandardStreamReader(Stream.Null, new UTF8Encoding(false, true), isRedirected: true, bounedConsole: null);

        /// <inheritdoc/>
        public IConsole BoundedConsole { get; }

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool isRedirected, IConsole bounedConsole) :
            base(stream)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks, bool isRedirected, IConsole bounedConsole) :
            base(stream, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen, bool isRedirected, IConsole bounedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
            BoundedConsole = bounedConsole;
        }
    }
}
