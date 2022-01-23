namespace Typin.Console.Internal
{
    using System.Text;

    internal static class NoPreambleEncodingExtensions
    {
        public static Encoding WithoutPreamble(this Encoding encoding)
        {
            return encoding.GetPreamble().Length > 0
                ? new NoPreambleEncoding(encoding)
                : encoding;
        }
    }
}
