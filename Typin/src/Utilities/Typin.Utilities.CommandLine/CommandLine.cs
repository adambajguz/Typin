namespace Typin.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Command line utils.
    /// </summary>
    public static class CommandLine
    {
        /// <summary>
        /// Encodes arguments using <see cref="EncodeArgument(string, bool)"/>.
        /// </summary>
        /// <param name="arguments">Supplies the arguments to encode.</param>
        /// <param name="forceQuotes">
        /// Supplies an indication of whether we should quote the argument even if it
        /// does not contain any characters that would ordinarily require quoting.
        /// </param>
        /// <returns></returns>
        public static string EncodeArguments(IEnumerable<string> arguments, bool forceQuotes = false)
        {
            StringBuilder s = new();
            foreach (string argument in arguments)
            {
                if (s.Length > 0)
                {
                    s.Append(' ');
                }

                s.Append(EncodeArgument(argument, forceQuotes));
            }

            return s.ToString();
        }

        /// <summary>
        /// This routine appends the given argument to a command line such that
        /// CommandLineToArgvW will return the argument string unchanged. Arguments
        /// in a command line should be separated by spaces; this function does
        /// not add these spaces.
        /// </summary>
        /// <param name="argument">Supplies the argument to encode.</param>
        /// <param name="forceQuotes">
        /// Supplies an indication of whether we should quote the argument even if it
        /// does not contain any characters that would ordinarily require quoting.
        /// </param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/37646895</remarks>
        public static string EncodeArgument(string argument, bool forceQuotes = false)
        {
            _ = argument ?? throw new ArgumentNullException(nameof(argument));

            // Unless we're told otherwise, don't quote unless we actually
            // need to do so --- hopefully avoid problems if programs won't
            // parse quotes properly
            if (forceQuotes == false
                && argument.Length > 0
                && argument.IndexOfAny(" \t\n\v\"".ToCharArray()) == -1)
            {
                return argument;
            }

            StringBuilder quoted = new();
            quoted.Append('"');

            int numberBackslashes = 0;

            foreach (char chr in argument)
            {
                switch (chr)
                {
                    case '\\':
                        numberBackslashes++;
                        continue;
                    case '"':
                        // Escape all backslashes and the following
                        // double quotation mark.
                        quoted.Append('\\', (numberBackslashes * 2) + 1);
                        quoted.Append(chr);
                        break;
                    default:
                        // Backslashes aren't special here.
                        quoted.Append('\\', numberBackslashes);
                        quoted.Append(chr);
                        break;
                }

                numberBackslashes = 0;
            }

            // Escape all backslashes, but let the terminating
            // double quotation mark we add below be interpreted
            // as a meta-character.
            quoted.Append('\\', numberBackslashes * 2);
            quoted.Append('"');

            return quoted.ToString();
        }

        /// <summary>
        /// Splits command line into parameters.
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
            for (int i = 0; i < commandLine.Length; i++)
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
