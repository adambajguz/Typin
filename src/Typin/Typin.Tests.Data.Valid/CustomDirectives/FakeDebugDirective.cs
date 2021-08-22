namespace Typin.Directives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Console;

    [Directive(BuiltInDirectives.Debug, Description = "FAKE: Starts a debugging mode. Application will wait for debugger to be attached before proceeding.")]
    public sealed class FakeDebugDirective : IPipelinedDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
#if NET5_0
            int processId = Environment.ProcessId;
#else
            int processId = Process.GetCurrentProcess().Id;
#endif

            args.Console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine($"Attach debugger to PID {processId} to continue."));

            await next();
        }
    }
}
