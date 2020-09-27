namespace Typin.Exceptions
{
    using System;
    using Typin.Console;
    using Typin.HelpWriter;

    /// <summary>
    /// Implementation of <see cref="ICliExceptionHandler"/> that prints all exceptions to console.
    /// </summary>
    public class DefaultExceptionHandler : ICliExceptionHandler
    {
        /// <inheritdoc/>
        public void HandleTypinException(ICliContext context, TypinException ex)
        {
            WriteError(context.Console, ex.ToString());

            //PrintHelp(context);
        }

        /// <inheritdoc/>
        public void HandleDirectiveException(ICliContext context, DirectiveException ex)
        {
            WriteError(context.Console, ex.ToString());
            context.Console.Error.WriteLine();

            if (ex.ShowHelp)
                PrintHelp(context);
        }

        /// <inheritdoc/>
        public void HandleCommandException(ICliContext context, CommandException ex)
        {
            WriteError(context.Console, ex.ToString());
            context.Console.Error.WriteLine();

            if (ex.ShowHelp)
                PrintHelp(context);
        }

        /// <inheritdoc/>
        public void HandleException(ICliContext context, Exception ex)
        {
            IConsole console = context.Console;

            WriteFatalError(console, $"Fatal error occured in {context.Metadata.ExecutableName}.");

            console.Error.WriteLine();
            WriteFatalError(console, ex.ToString());
            console.Error.WriteLine();
        }

        /// <summary>
        /// Write an error message to the console.
        /// </summary>
        private static void WriteError(IConsole console, string message)
        {
            console.WithForegroundColor(ConsoleColor.Red, () => console.Error.WriteLine(message));
        }

        /// <summary>
        /// Write a fataal error message to the console.
        /// </summary>
        private static void WriteFatalError(IConsole console, string message)
        {
            console.WithForegroundColor(ConsoleColor.DarkRed, () => console.Error.WriteLine(message));
        }

        private static void PrintHelp(ICliContext context)
        {
            IHelpTextWriter helpTextWriter = new DefaultHelpTextWriter(context);
            helpTextWriter.Write();
        }
    }
}
