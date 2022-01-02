namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;

    /// <summary>
    /// A middleware that handles all <see cref="CliException"/> exceptions.
    /// </summary>
    public sealed class TypinExceptionsHandler : IMiddleware
    {
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of <see cref="TypinExceptionsHandler"/>.
        /// </summary>
        public TypinExceptionsHandler(IConsole console)
        {
            _console = console;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            try
            {
                await next();
            }
            catch (CliException ex)
            {
                _console.Error.WithForegroundColor(ConsoleColor.Red, (o) =>
                {
                    o.WriteLine(ex.Message);
                    o.WriteLine();
                });
            }
        }
    }
}
