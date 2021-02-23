namespace Typin.Console.IO
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="StreamReader"/> wrapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StandardStreamReader : StreamReader, IStandardRedirectableConsoleStream
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

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool isRedirected, IConsole boundedConsole) :
            base(stream)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks, bool isRedirected, IConsole boundedConsole) :
            base(stream, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool isRedirected, IConsole boundedConsole) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, bool isRedirected, IConsole boundedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool isRedirected, IConsole boundedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen, bool isRedirected, IConsole boundedConsole) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
            BoundedConsole = boundedConsole;
        }
    }
}
