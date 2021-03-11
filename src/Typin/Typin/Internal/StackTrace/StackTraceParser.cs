namespace Typin.Internal.StackTrace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Typin.Internal.Extensions;

    internal class StackTraceParser
    {
        private const string Space = @"[\x20\t]";
        private const string NotSpace = @"[^\x20\t]";

        // Taken from https://github.com/atifaziz/StackTraceParser
        private static readonly Regex Pattern = new(@"
            ^
            " + Space + @"*
            \w+ " + Space + @"+
            (?<frame>
                (?<type> " + NotSpace + @"+ ) \.
                (?<method> " + NotSpace + @"+? ) " + Space + @"*
                (?<params>  \( ( " + Space + @"* \)
                               |                    (?<pt> .+?) " + Space + @"+ (?<pn> .+?)
                                 (, " + Space + @"* (?<pt> .+?) " + Space + @"+ (?<pn> .+?) )* \) ) )
                ( " + Space + @"+
                    ( # Microsoft .NET stack traces
                    \w+ " + Space + @"+
                    (?<file> ( [a-z] \: # Windows rooted path starting with a drive letter
                             | / )      # *nix rooted path starting with a forward-slash
                             .+? )
                    \: \w+ " + Space + @"+
                    (?<line> [0-9]+ ) \p{P}?
                    | # Mono stack traces
                    \[0x[0-9a-f]+\] " + Space + @"+ \w+ " + Space + @"+
                    <(?<file> [^>]+ )>
                    :(?<line> [0-9]+ )
                    )
                )?
            )
            \s*
            $",
            RegexOptions.IgnoreCase |
            RegexOptions.Multiline |
            RegexOptions.ExplicitCapture |
            RegexOptions.CultureInvariant |
            RegexOptions.IgnorePatternWhitespace,
            TimeSpan.FromSeconds(5)
        );

        public static IEnumerable<StackFrame> ParseMany(string stackTrace)
        {
            Match[] matches = Pattern.Matches(stackTrace).Cast<Match>().ToArray();

            // Ensure success (all lines should be parsed)
            bool isSuccess = matches.Length == stackTrace.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;

            if (!isSuccess)
            {
                throw new FormatException("Could not parse stack trace.");
            }

            foreach (Match m in matches)
            {
                GroupCollection groups = m.Groups;
                CaptureCollection pt = groups["pt"].Captures;
                CaptureCollection pn = groups["pn"].Captures;

                yield return new StackFrame(
                    groups["type"].Value,
                    groups["method"].Value,
                    (
                        from i in Enumerable.Range(0, pt.Count)
                        select new StackFrameParameter(pt[i].Value, pn[i].Value.NullIfEmpty())
                    ).ToArray(),
                    groups["file"].Value.NullIfEmpty(),
                    groups["line"].Value.NullIfEmpty()
                );
            }

            yield break;
        }
    }
}
