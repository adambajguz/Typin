namespace Typin.Tests.Data.Middlewares
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        public const string ExpectedOutput0 = "-- Handling Command";
        public const string ExpectedOutput1 = "-- Finished Command after";

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.Console.Output.WriteLine(ExpectedOutput0);
            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();
            context.Console.Output.WriteLine($"{ExpectedOutput1} {0} ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
