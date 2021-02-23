namespace InteractiveModeExample.Middlewares
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
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
