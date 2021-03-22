namespace Typin.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Typin.Console;
    using Typin.Console.IO;
    using Typin.Extensions;

    /// <summary>
    /// Simple table utils for console.
    /// </summary>
    public static class TableUtils
    {
        /// <summary>
        /// Writes a table to the console.
        /// </summary>
        public static void Write<TElement>(StandardStreamWriter stream,
                                           IEnumerable<TElement> collection,
                                           IEnumerable<string> headers,
                                           string? footnotes,
                                           params Expression<Func<TElement, string>>[] values)
        {
            int columnsCount = values.Length;

            Func<TElement, string>[] columnFunctions = values.Select(x => x.Compile())
                                                             .ToArray();

            int[] columnWidths = (from cf in columnFunctions
                                  let x = (collection.Any() ? collection.Select(cf).Max(x => x.Length) : 0)
                                  select x).ToArray();

            AdjustColumnWidths(headers, columnsCount, columnWidths);

            int totalWidth = columnWidths.Sum() - 1 + (columnsCount) * 3;

            //Write top border
            stream.WriteBorder(totalWidth);
            stream.WriteTableHeader(headers, columnWidths, totalWidth);

            //Write table body
            stream.WriteTableBody(collection, columnFunctions, columnWidths);

            // Write bottom border and try write footnotes
            stream.WriteBorder(totalWidth);
            stream.TryWriteFootnotes(footnotes);
        }

        /// <summary>
        /// Writes a table to the console.
        /// </summary>
        public static void Write<TKey, TElement>(StandardStreamWriter stream,
                                                 IEnumerable<IGrouping<TKey, TElement>> collection,
                                                 IEnumerable<string> headers,
                                                 string? footnotes,
                                                 params Expression<Func<TElement, string>>[] values)
        {
            int columnsCount = values.Length;

            Func<TElement, string>[] columnFunctions = values.Select(x => x.Compile())
                                                          .ToArray();

            int[] columnWidths = (from cf in columnFunctions
                                  let x = (collection.Any() ? collection.SelectMany(x => x).Select(cf).Max(x => x.Length) : 0)
                                  select x).ToArray();

            AdjustColumnWidths(headers, columnsCount, columnWidths);

            int totalWidth = columnWidths.Sum() - 1 + (columnsCount) * 3;

            //Write top border
            stream.WriteBorder(totalWidth);
            stream.WriteTableHeader(headers, columnWidths, totalWidth);

            foreach (IGrouping<TKey, TElement> group in collection)
            {
                TKey groupKey = group.Key;
                int countInGroup = group.Count();

                stream.WriteBorder(totalWidth, '-');
                stream.WithForegroundColor(ConsoleColor.DarkYellow, (o) => o.WriteLine($" {groupKey} ({countInGroup}) ".PadBoth(totalWidth)));
                stream.WriteBorder(totalWidth, '-');

                //Write table body
                stream.WriteTableBody(group, columnFunctions, columnWidths);
            }

            // Write bottom border and try write footnotes
            stream.WriteBorder(totalWidth);
            stream.TryWriteFootnotes(footnotes);
        }

        #region Helpers
        private static void WriteBorder(this StandardStreamWriter stream, int totalWidth, char ch = '=')
        {
            stream.WithForegroundColor(ConsoleColor.Magenta, (o) => o.WriteLine(new string(ch, totalWidth)));
        }

        private static void TryWriteFootnotes(this StandardStreamWriter stream, string? footnotes)
        {
            if (!string.IsNullOrWhiteSpace(footnotes))
            {
                stream.WithForegroundColor(ConsoleColor.DarkGray, (o) => o.WriteLine(TextUtils.AdjustNewLines(footnotes)));
                stream.WriteLine();
            }
        }

        private static void WriteTableHeader(this StandardStreamWriter stream, IEnumerable<string> headers, int[] columnWidths, int totalWidth)
        {
            if (headers.Any())
            {
                for (int i = 0; i < columnWidths.Length; ++i)
                {
                    string header = headers.ElementAtOrDefault(i) ?? string.Empty;
                    int targetWidth = columnWidths[i];

                    stream.Write(' ');
                    stream.WithForegroundColor(ConsoleColor.DarkYellow, (o) => o.Write(header.PadRight(targetWidth)));

                    if (i + 1 < columnWidths.Length)
                    {
                        stream.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write(" |"));
                    }
                }
                stream.WriteLine();

                //Write middle line
                stream.WriteBorder(totalWidth);
            }
        }

        private static void WriteTableBody<TElement>(this StandardStreamWriter stream, IEnumerable<TElement> collection, Func<TElement, string>[] columnFunctions, int[] columnWidths)
        {
            foreach (TElement item in collection)
            {
                for (int i = 0; i < columnWidths.Length; ++i)
                {
                    Func<TElement, string> column = columnFunctions[i];
                    stream.Write(' ');

                    string value = columnFunctions[i].Invoke(item);
                    int targetWidth = columnWidths[i];

                    stream.Write(value.PadRight(targetWidth));

                    if (i + 1 < columnWidths.Length)
                    {
                        stream.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write(" |"));
                    }
                }

                stream.WriteLine();
            }
        }

        private static void AdjustColumnWidths(IEnumerable<string> headers, int columnsCount, int[] columnWidths)
        {
            //Update column widths for smaller than header length
            for (int i = 0; i < columnsCount; ++i)
            {
                string header = headers.ElementAtOrDefault(i) ?? string.Empty;

                if (columnWidths[i] < header.Length)
                {
                    columnWidths[i] = header.Length;
                }
            }
        }
        #endregion
    }
}
