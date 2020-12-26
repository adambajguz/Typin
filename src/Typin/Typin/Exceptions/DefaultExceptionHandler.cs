namespace Typin.Exceptions
{
    using System;
    using Typin.Console;
    using Typin.Help;

    /// <summary>
    /// Implementation of <see cref="ICliExceptionHandler"/> that prints all exceptions to console.
    /// </summary>
    public class DefaultExceptionHandler : ICliExceptionHandler
    {
        private readonly IConsole _console;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of <see cref="DefaultExceptionHandler"/>.
        /// </summary>
        public DefaultExceptionHandler(IConsole console, IServiceProvider serviceProvider)
        {
            _console = console;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public bool HandleException(Exception ex)
        {
            IConsole console = _console;

            switch (ex)
            {
                // Swallow directive exceptions and route them to the console
                case CommandException cx:
                    {
                        WriteError(console, cx.Message);

                        if (cx.ShowHelp)
                        {
                            (_serviceProvider.GetService(typeof(IHelpWriter)) as IHelpWriter)?.Write();
                        }
                    }
                    return true;

                // Swallow command exceptions and route them to the console
                case DirectiveException dx:
                    {
                        WriteError(console, dx.Message);

                        if (dx.ShowHelp)
                        {
                            (_serviceProvider.GetService(typeof(IHelpWriter)) as IHelpWriter)?.Write();
                        }
                    }
                    return true;

                // This may throw exceptions which are useful only to the end-user
                case TypinException tx:
                    WriteError(console, tx.Message);

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
            console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine(message));
            console.Error.WriteLine();
        }
    }
}
