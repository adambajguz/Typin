﻿namespace InteractiveModeExample.Middlewares
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Commands.Features;
    using Typin.Console;

    public sealed class ExecutionTimingMiddleware : IMiddleware
    {
        private readonly IConsole _console;

        public ExecutionTimingMiddleware(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
            {
                output.WriteLine($"--- Handling command '{args.Features.Get<ICommandFeature>()?.Schema.Aliases}' with args '{string.Join(' ', args.Input.Original ?? Array.Empty<string>())}'");
            });

            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            int? exitCode = args.Output.ExitCode;
            if (exitCode == 0)
            {
                _console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
                {
                    output.WriteLine("--- Command finished successfully after {0} ms.",
                                     stopwatch.Elapsed.TotalMilliseconds);
                });
            }
            else
            {
                _console.Output.WithForegroundColor(ConsoleColor.DarkGray, (output) =>
                {
                    output.WriteLine("--- Command finished with exit code ({0}) after {1} ms.",
                                     args.Output.ExitCode ?? ExitCode.Error,
                                     stopwatch.Elapsed.TotalMilliseconds);
                });
            }

        }
    }
}