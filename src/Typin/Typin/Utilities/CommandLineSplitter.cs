namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Command line splitter.
    /// </summary>
    public static class CommandLineSplitter
    {
        /// <summary>
        /// Splits command line.
        /// </summary>
        public static IEnumerable<string> Split(string commandLine)
        {
            var result = new StringBuilder();

            bool quoted = false;
            bool escaped = false;
            bool started = false;
            bool allowCaret = false;

            for (int i = 0; i < commandLine.Length; ++i)
            {
                char chr = commandLine[i];
                bool v = i + 1 < commandLine.Length;

                if (chr == '^' && !quoted)
                {
                    if (allowCaret)
                    {
                        result.Append(chr);
                        started = true;
                        escaped = false;
                        allowCaret = false;
                    }
                    else if (v && commandLine[i + 1] == '^')
                    {
                        allowCaret = true;
                    }
                    else if (v)
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
                else if (chr == '\\' && v && commandLine[i + 1] == '"')
                {
                    escaped = true;
                }
                else if (chr == ' ' && !quoted)
                {
                    if (started)
                        yield return result.ToString();

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
                yield return result.ToString();
        }
    }
}
