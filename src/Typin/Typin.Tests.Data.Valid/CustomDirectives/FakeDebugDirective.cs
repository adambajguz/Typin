namespace Typin.Tests.Data.Valid.CustomDirectives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive(BuiltInDirectives.Debug, Description = "FAKE: Starts a debugging mode. Application will wait for debugger to be attached before proceeding.")]
    public sealed class FakeDebugDirective : IDirective
    {
        private readonly IConsole _console;

        public FakeDebugDirective(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            int processId = Environment.ProcessId;

            _console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine($"Attach debugger to PID {processId} to continue."));

            await next();
        }
    }
}
