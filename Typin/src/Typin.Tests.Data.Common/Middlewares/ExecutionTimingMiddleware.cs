namespace Typin.Tests.Data.Common.Middlewares
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        public const string ExpectedOutput0 = "-- Handling Command";
        public const string ExpectedOutput1 = "-- Finished Command after";

        private readonly IConsole _console;

        public ExecutionTimingMiddleware(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.WriteLine(ExpectedOutput0);
            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();
            _console.Output.WriteLine($"{ExpectedOutput1} {0} ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
