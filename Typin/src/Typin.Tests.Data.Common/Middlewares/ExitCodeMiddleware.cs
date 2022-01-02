namespace Typin.Tests.Data.Common.Middlewares
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;

    public sealed class ExitCodeMiddleware : IMiddleware
    {
        public const string ExpectedOutput = "Command finished succesfully.";

        private readonly IConsole _console;

        public ExitCodeMiddleware(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            await next();
            int? exitCode = args.Output.ExitCode;

            if (exitCode == 0)
            {
                _console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                    output.WriteLine($"{args.Call.Identifier}: {ExpectedOutput}."));
            }
            else
            {
                _console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                    output.WriteLine($"{args.Call.Identifier}: Command finished with exit code ({exitCode})."));
            }
        }
    }
}
