namespace Typin.Utilities
{
    using System.IO;
    using System.Text;
    using Typin.Console;

    /// <summary>
    /// Implementation of <see cref="StreamWriter"/> with a <see cref="MemoryStream"/> as a backing store.
    /// </summary>
    public class MemoryStreamWriter : StandardStreamWriter
    {
        /// <summary>
        /// Gets the underlying stream that interfaces with a backing store.
        /// </summary>
        private new MemoryStream BaseStream => (MemoryStream)base.BaseStream;

        /// <summary>
        /// Initializes an instance of <see cref="MemoryStreamWriter"/>.
        /// </summary>
        public MemoryStreamWriter(Encoding encoding, bool isRedirected)
            : base(new MemoryStream(), encoding, isRedirected)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="MemoryStreamWriter"/>.
        /// </summary>
        public MemoryStreamWriter(bool isRedirected)
            : base(new MemoryStream(), isRedirected)
        {

        }

        /// <summary>
        /// Gets the bytes written to the underlying stream.
        /// </summary>
        public byte[] GetBytes()
        {
            Flush();
            return BaseStream.ToArray();
        }

        /// <summary>
        /// Gets the string written to the underlying stream.
        /// </summary>
        public string GetString()
        {
            return Encoding.GetString(GetBytes());
        }
    }
}
