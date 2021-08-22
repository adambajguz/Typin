namespace Typin.Tests.Data.Middlewares
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

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            await next();
            int? exitCode = args.ExitCode;

            if (args.ExitCode == 0)
            {
                args.Console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                    output.WriteLine($"{args.Metadata.ExecutableName}: {ExpectedOutput}."));
            }
            else
            {
                args.Console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                    output.WriteLine($"{args.Metadata.ExecutableName}: Command finished with exit code ({exitCode})."));
            }
        }
    }
}
