namespace Typin.Utilities
{
    using System;
    using System.IO;
    using Typin.Console;
    using Typin.Console.IO;
    using Typin.Internal.StackTrace;

    /// <summary>
    /// Formatted exception writer.
    /// </summary>
    public static class ExceptionFormatter
    {
        /// <summary>
        /// Writes formatted exception to output stream (stdout or stderr).
        /// </summary>
        public static void WriteException(this StandardStreamWriter output, Exception exception)
        {
            output.WriteException(exception, 0);
        }

        private static void WriteException(this StandardStreamWriter output, Exception exception, int indentLevel)
        {
            Type exceptionType = exception.GetType();

            string indentationShared = new(' ', 4 * indentLevel);
            string indentationLocal = new(' ', 2);

            // Fully qualified exception type
            output.Write(indentationShared);
            output.WithForegroundColor(ConsoleColor.DarkGray, (o) =>
            {
                o.Write(exceptionType.Namespace);
                o.Write('.');
            });
            output.WithForegroundColor(ConsoleColor.White, (o) => o.Write(exceptionType.Name));
            output.Write(": ");

            // Exception message
            output.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine(exception.Message));

            // Recurse into inner exceptions
            if (exception.InnerException is not null)
            {
                output.WriteException(exception.InnerException, indentLevel + 1);
            }

            if (exception.StackTrace is null)
            {
                return;
            }

            // Try to parse and pretty-print the stack trace
            try
            {
                foreach (StackFrame stackFrame in StackTraceParser.ParseMany(exception.StackTrace))
                {
                    output.Write(indentationShared);
                    output.Write(indentationLocal);
                    output.WithForegroundColor(ConsoleColor.Green, (o) => o.Write("at "));

                    // "Typin.Demo.Commands.BookAddCommand."
                    output.WithForegroundColor(ConsoleColor.DarkGray, (o) =>
                    {
                        o.Write(stackFrame.ParentType);
                        o.Write('.');
                    });

                    // "ExecuteAsync"
                    output.WithForegroundColor(ConsoleColor.Yellow, (o) => o.Write(stackFrame.MethodName));

                    output.Write('(');

                    for (int i = 0; i < stackFrame.Parameters.Count; ++i)
                    {
                        StackFrameParameter parameter = stackFrame.Parameters[i];

                        // "IConsole"
                        output.WithForegroundColor(ConsoleColor.Blue, (o) => o.Write(parameter.Type));

                        if (!string.IsNullOrWhiteSpace(parameter.Name))
                        {
                            output.Write(' ');

                            // "console"
                            output.WithForegroundColor(ConsoleColor.White, (o) => o.Write(parameter.Name));
                        }

                        // Separator
                        if (stackFrame.Parameters.Count > 1 && i < stackFrame.Parameters.Count - 1)
                        {
                            output.Write(", ");
                        }
                    }

                    output.Write(") ");

                    // Location
                    if (!string.IsNullOrWhiteSpace(stackFrame.FilePath))
                    {
                        output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write("in"));
                        output.WriteLine();
                        output.Write(indentationShared);
                        output.Write(indentationLocal);
                        output.Write(indentationLocal);

                        // "C:\Projects\Typin\Typin.Demo\Commands\"
                        var stackFrameDirectoryPath = Path.GetDirectoryName(stackFrame.FilePath);
                        output.WithForegroundColor(ConsoleColor.DarkGray, (o) =>
                        {
                            o.Write(stackFrameDirectoryPath);
                            o.Write(Path.DirectorySeparatorChar);
                        });

                        // "BookAddCommand.cs"
                        string stackFrameFileName = Path.GetFileName(stackFrame.FilePath) ?? string.Empty;
                        output.WithForegroundColor(ConsoleColor.Cyan, (o) => o.Write(stackFrameFileName));

                        if (!string.IsNullOrWhiteSpace(stackFrame.LineNumber))
                        {
                            output.Write(':');

                            // "35"
                            output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write(stackFrame.LineNumber));
                        }
                    }

                    output.WriteLine();
                }
            }
            // If any point of parsing has failed - print the stack trace without any formatting
            catch
            {
                output.WriteLine(exception.StackTrace);
            }
        }
    }
}
