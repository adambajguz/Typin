namespace Typin.Directives
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Console;

    /// <summary>
    /// When application runs in debug mode (using the [debug] directive), it will wait for debugger to be attached before proceeding.
    /// This is useful for debugging apps that were ran outside of the IDE.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.Debug, Description = "Starts a debugging mode. Application will wait for debugger to be attached before proceeding.")]
    public sealed class DebugDirective : IPipelinedDirective
    {
        /// <inheritdoc/>
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
#if NET5_0
            int processId = Environment.ProcessId;
#else
            int processId = Process.GetCurrentProcess().Id;
#endif

            IConsole console = args.Console;

            console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine($"Attach debugger to PID {processId} to continue."));

            Debugger.Launch();

            while (!Debugger.IsAttached)
            {
                await Task.Delay(100, cancellationToken);
            }

            //Replace with an event
            //console.WithForegroundColor(ConsoleColor.Green, () =>
            //    console.Output.WriteLine($"Debugger attached to PID {processId}."));

            await next();
        }
    }
}
