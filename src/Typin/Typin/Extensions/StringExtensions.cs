namespace Typin.Extensions
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string that right-aligns the characters in this instance by padding them on the left and right with a specified Unicode character, for a specified total length.
        /// </summary>
        public static string PadBoth(this string source, int totalWidth, char paddingChar = ' ')
        {
            int spaces = totalWidth - source.Length;
            int padLeft = spaces / 2 + source.Length;

            return source.PadLeft(padLeft, paddingChar)
                         .PadRight(totalWidth, paddingChar);
        }
    }
}