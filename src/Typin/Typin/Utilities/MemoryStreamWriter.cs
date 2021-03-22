namespace Typin.Utilities
{
    using System.IO;
    using System.Text;
    using Typin.Console;

    /// <summary>
    /// Wrapper over a <see cref="MemoryStream"/> for <see cref="VirtualConsole"/>.
    /// </summary>
    public class MemoryStreamWriter
    {
        /// <summary>
        /// Gets the stream that interfaces with a backing store.
        /// </summary>
        public MemoryStream Stream { get; } = new();

        /// <summary>
        /// Gets the stream that interfaces with a backing store.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Initializes an instance of <see cref="MemoryStreamWriter"/>.
        /// </summary>
        public MemoryStreamWriter(Encoding encoding)
        {
            Encoding = encoding;
        }

        /// <summary>
        /// Gets the bytes written to the underlying stream.
        /// </summary>
        public byte[] GetBytes()
        {
            Stream.Flush();
            return Stream.ToArray();
        }

        /// <summary>
        /// Gets the string written to the underlying stream.
        /// </summary>
        public string GetString()
        {
            byte[] bytes = GetBytes();
            return Encoding.GetString(bytes);
        }

        /// <summary>
        /// Gets the string written to the underlying stream.
        /// </summary>
        public string GetString(Encoding encoding)
        {
            byte[] bytes = GetBytes();
            return encoding.GetString(bytes);
        }
    }
}
