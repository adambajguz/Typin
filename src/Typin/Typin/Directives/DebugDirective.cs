namespace Typin.Directives
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
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
        public async ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            int processId = Process.GetCurrentProcess().Id;
            IConsole console = context.Console;

            console.WithForegroundColor(ConsoleColor.Green, () =>
                console.Output.WriteLine($"Attach debugger to PID {processId} to continue."));

            Debugger.Launch();

            while (!Debugger.IsAttached)
                await Task.Delay(100, cancellationToken);

            //Replace with an event
            console.WithForegroundColor(ConsoleColor.Green, () =>
                console.Output.WriteLine($"Debugger attached to PID {processId}."));

            await next();
        }
    }
}
