namespace Typin.Utilities
{
    using System.IO;
    using System.Text;
    using Typin.Console;

    /// <summary>
    /// Wrapper over a <see cref="MemoryStream"/> for <see cref="VirtualConsole"/>.
    /// </summary>
    public class MemoryStreamReader
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
        public MemoryStreamReader(Encoding encoding)
        {
            Encoding = encoding;
        }

        /// <summary>
        /// Writes a string to the underlying stream.
        /// </summary>
        public void WriteString(string text)
        {
            byte[] bytes = Encoding.GetBytes(text);
            Stream.Write(bytes, 0, bytes.Length);
            Stream.Position = 0;
        }
    }
}
