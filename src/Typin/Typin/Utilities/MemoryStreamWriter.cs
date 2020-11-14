namespace Typin.Utilities
{
    using System.IO;
    using System.Text;
    using Typin.Console;
    using Typin.Console.IO;

    /// <summary>
    /// Implementation of <see cref="StreamWriter"/> with a <see cref="MemoryStream"/> as a backing store.
    /// </summary>
    public class MemoryStreamWriter
    {
        /// <summary>
        /// Gets the underlying stream that interfaces with a backing store.
        /// </summary>
        public MemoryStream BaseStream { get; } = new MemoryStream();

        /// <summary>
        /// Initializes an instance of <see cref="MemoryStreamWriter"/>.
        /// </summary>
        public MemoryStreamWriter()
        {

        }

        /// <summary>
        /// Gets the bytes written to the underlying stream.
        /// </summary>
        public byte[] GetBytes()
        {
            BaseStream.Flush();
            return BaseStream.ToArray();
        }

        /// <summary>
        /// Gets the string written to the underlying stream.
        /// </summary>
        public string GetString()
        {
            byte[] bytes = GetBytes();
            return Encoding.GetString(bytes);
        }
    }
}
