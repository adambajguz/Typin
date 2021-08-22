namespace Typin.Tests.Data.Middlewares
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        public const string ExpectedOutput0 = "-- Handling Command";
        public const string ExpectedOutput1 = "-- Finished Command after";

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Console.Output.WriteLine(ExpectedOutput0);
            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();
            args.Console.Output.WriteLine($"{ExpectedOutput1} {0} ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
