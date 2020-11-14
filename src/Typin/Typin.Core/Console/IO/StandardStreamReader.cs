namespace Typin.Console
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="StreamReader"/> wrapper.
    /// </summary>
    public class StandardStreamReader : StreamReader
    {
        /// <summary>
        /// A <see cref="StandardStreamReader"/> object around an empty stream.
        /// </summary>
        public static new readonly StandardStreamReader Null = new StandardStreamReader(Stream.Null, new UTF8Encoding(false, true), isRedirected: true);

        /// <inheritdoc/>
        public bool IsRedirected { get; }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool isRedirected) :
            base(stream)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(string path, bool isRedirected) :
            base(path)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks, bool isRedirected) :
            base(stream, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool isRedirected) :
            base(stream, encoding)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(string path, bool detectEncodingFromByteOrderMarks, bool isRedirected) :
            base(path, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(string path, Encoding encoding, bool isRedirected) :
            base(path, encoding)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, bool isRedirected) :
            base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, bool isRedirected) :
            base(path, encoding, detectEncodingFromByteOrderMarks)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool isRedirected) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool isRedirected) :
            base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            IsRedirected = isRedirected;
        }

        /// <summary>
        /// Initializes an instance of <see cref="StandardStreamReader"/>.
        /// </summary>
        public StandardStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen, bool isRedirected) :
            base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
            IsRedirected = isRedirected;
        }
    }
}
