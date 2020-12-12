namespace TypinExamples.Timer.Middleware
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;
    using TypinExamples.Timer.Directives;
    using TypinExamples.Timer.Models;
    using TypinExamples.Timer.Repositories;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        private readonly IPerformanceLogsRepository _performanceLogsRepository;

        public ExecutionTimingMiddleware(IPerformanceLogsRepository performanceLogsRepository)
        {
            _performanceLogsRepository = performanceLogsRepository;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            bool hasPrintPerfDirective = context.GetDirectiveInstance<PrintPerformanceDirective>() is not null;
            bool hasNoLogDirective = context.GetDirectiveInstance<NoLoggingDirective>() is not null;

            if (hasPrintPerfDirective)
            {
                PrintExecutionBegin(context);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            if (hasNoLogDirective)
            {
                await next();
            }
            else
            {
                DateTime startedOn = DateTime.Now;

                await next();

                stopwatch.Stop();

                PerformanceLog log = new()
                {
                    StartedOn = startedOn,
                    CommandName = context.CommandSchema.Name,
                    Input = context.Input.Arguments,
                    Time = stopwatch.Elapsed
                };
                _performanceLogsRepository.Insert(log);
            }

            if (hasPrintPerfDirective)
            {
                PrintExecutionFinish(context, stopwatch);
            }
        }

        private static void PrintExecutionFinish(ICliContext context, Stopwatch stopwatch)
        {
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
                    context.Console.Output.WriteLine("--- Command finished with exit code ({0}) after {1} ms.",
                                                     context.ExitCode ?? ExitCodes.Error,
                                                     stopwatch.Elapsed.TotalMilliseconds);
                });
            }
        }

        private static void PrintExecutionBegin(ICliContext context)
        {
            context.Console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
            {
                output.WriteLine("--- Handling command '{0}' with args '{1}'",
                                 context.Input.CommandName ?? "<default>",
                                 string.Join(' ', context.Input.Arguments));
            });
        }
    }
}
