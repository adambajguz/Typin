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
    /// Handle exceptions.
    /// </summary>
    public sealed class HandleExceptions : IMiddleware
    {
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of <see cref="HandleExceptions"/>.
        /// </summary>
        public HandleExceptions(IConsole console)
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
