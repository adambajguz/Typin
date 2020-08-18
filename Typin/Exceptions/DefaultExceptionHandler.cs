namespace Typin.Exceptions
{
    using System;

    /// <summary>
    /// Implementation of <see cref="ICliExceptionHandler"/> that prints all exceptions to console.
    /// </summary>
    public class DefaultExceptionHandler : ICliExceptionHandler
    {
        /// <inheritdoc/>
        public void HandleTypinException(ICliContext context, TypinException ex)
        {
            WriteError(context.Console, ex.ToString());
            context.Console.Error.WriteLine();

            //ex.ShowHelp = false;
        }

        /// <inheritdoc/>
        public void HandleDirectiveException(ICliContext context, DirectiveException ex)
        {
            WriteError(context.Console, ex.ToString());
            context.Console.Error.WriteLine();
        }

        /// <inheritdoc/>
        public void HandleCommandException(ICliContext context, CommandException ex)
        {
            WriteError(context.Console, ex.ToString());
            context.Console.Error.WriteLine();
        }

        /// <inheritdoc/>
        public void HandleException(ICliContext context, Exception ex)
        {
            IConsole console = context.Console;

            WriteError(console, $"Fatal error occured in {context.Metadata.ExecutableName}.");

            console.Error.WriteLine();
            WriteError(console, ex.ToString());
            console.Error.WriteLine();
        }

        /// <summary>
        /// Write an error message to the console.
        /// </summary>
        protected static void WriteError(IConsole console, string message)
        {
            console.WithForegroundColor(ConsoleColor.Red, () => console.Error.WriteLine(message));
        }
    }
}
