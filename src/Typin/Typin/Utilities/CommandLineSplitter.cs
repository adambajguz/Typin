namespace Typin.Utilities
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Command line splitter.
    /// </summary>
    public static class CommandLineSplitter
    {
        /// <summary>
        /// Splits line into commands.
        /// </summary>
        /// <remarks>
        /// https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
        /// by Mikescher
        /// </remarks>
        public static IEnumerable<string> Split(string commandLine)
        {
            StringBuilder result = new();

            bool quoted = false;
            bool escaped = false;
            bool started = false;
            bool allowcaret = false;
            for (int i = 0; i < commandLine.Length; ++i)
            {
                char chr = commandLine[i];
                int iNext = i + 1;

                if (chr == '^' && !quoted)
                {
                    if (allowcaret)
                    {
                        result.Append(chr);
                        started = true;
                        escaped = false;
                        allowcaret = false;
                    }
                    else if (iNext < commandLine.Length && commandLine[iNext] == '^')
                    {
                        allowcaret = true;
                    }
                    else if (iNext == commandLine.Length)
                    {
                        result.Append(chr);
                        started = true;
                        escaped = false;
                    }
                }
                else if (escaped)
                {
                    result.Append(chr);
                    started = true;
                    escaped = false;
                }
                else if (chr == '"')
                {
                    quoted = !quoted;
                    started = true;
                }
                else if (chr == '\\' && iNext < commandLine.Length && commandLine[iNext] == '"')
                {
                    escaped = true;
                }
                else if (chr == ' ' && !quoted)
                {
                    if (started)
                    {
                        yield return result.ToString();
                    }

                    result.Clear();
                    started = false;
                }
                else
                {
                    result.Append(chr);
                    started = true;
                }
            }

            if (started)
            {
                yield return result.ToString();
            }
        }
    }
}
