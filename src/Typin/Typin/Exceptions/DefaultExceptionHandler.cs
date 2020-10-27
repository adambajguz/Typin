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
        private readonly ICliContext _cliContext;
        private readonly IHelpWriter _helpWriter;

        /// <summary>
        /// Initializes an instance of <see cref="DefaultExceptionHandler"/>.
        /// </summary>
        public DefaultExceptionHandler(ICliContext context, IHelpWriter helpWriter)
        {
            _cliContext = context;
            _helpWriter = helpWriter;
        }

        /// <inheritdoc/>
        public bool HandleException(Exception ex)
        {
            IConsole console = _cliContext.Console;

            switch (ex)
            {
                // Swallow directive exceptions and route them to the console
                case CommandException cx:
                    {
                        WriteError(console, cx.ToString());
                        console.Error.WriteLine();

                        if (cx.ShowHelp)
                            _helpWriter.Write();
                    }
                    return true;

                // Swallow command exceptions and route them to the console
                case DirectiveException dx:
                    {
                        WriteError(console, dx.ToString());
                        console.Error.WriteLine();

                        if (dx.ShowHelp)
                            _helpWriter.Write();
                    }
                    return true;

                // This may throw exceptions which are useful only to the end-user
                case TypinException tx:
                    WriteError(console, tx.ToString());
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Write an error message to the console.
        /// </summary>
        private static void WriteError(IConsole console, string message)
        {
            console.WithForegroundColor(ConsoleColor.Red, () => console.Error.WriteLine(message));
        }
    }
}
