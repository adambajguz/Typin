namespace InteractiveModeExample.Middlewares
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            var context = args;

            context.Console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
            {
                output.WriteLine("--- Handling command '{0}' with args '{1}'",
                                 context.Input.CommandName ?? "<default>",
                                 string.Join(' ', context.Input.Arguments));
            });

            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            int? exitCode = context.ExitCode;
            if (context.ExitCode == 0)
            {
                context.Console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
                {
                    output.WriteLine("--- Command finished successfully after {0} ms.",
                                     stopwatch.Elapsed.TotalMilliseconds);
                });
            }
            else
            {
                context.Console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
                {
                    output.WriteLine("--- Command finished with exit code ({0}) after {1} ms.",
                                     context.ExitCode ?? ExitCodes.Error,
                                     stopwatch.Elapsed.TotalMilliseconds);
                });
            }

        }
    }
}
