namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Exceptions;

    /// <summary>
    /// A middleware that handles all <see cref="TypinException"/> exceptions.
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
            catch (TypinException tex)
            {
                _console.Error.WithForegroundColor(ConsoleColor.Red, (o) =>
                {
                    o.WriteLine(tex.Message);
                    o.WriteLine();
                });
            }
        }
    }
}
