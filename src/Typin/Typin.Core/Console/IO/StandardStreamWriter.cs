namespace Typin.Console
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="StreamWriter"/> wrapper.
    /// </summary>
    public class StandardStreamWriter : StreamWriter, IStandardRedirectableStream
    {
        /// <summary>
        /// A <see cref="StandardStreamWriter"/> object around an empty stream.
        /// </summary>
        public static new readonly StandardStreamWriter Null = new StandardStreamWriter(Stream.Null, new UTF8Encoding(false, true), 128, true, isRedirected: true);

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, bool isRedirected) :
            base(stream)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(string path, bool isRedirected) :
            base(path)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, bool isRedirected) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(string path, bool append, bool isRedirected) :
            base(path, append)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool isRedirected) :
            base(stream, encoding, bufferSize)
        {
            IsRedirected = isRedirected;

        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(string path, bool append, Encoding encoding, bool isRedirected) :
            base(path, append, encoding)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen, bool isRedirected) :
            base(stream, encoding, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamWriter"/>.
        /// </summary>
        public StandardStreamWriter(string path, bool append, Encoding encoding, int bufferSize, bool isRedirected) :
            base(path, append, encoding, bufferSize)
        {
            IsRedirected = isRedirected;
        }
    }
}
