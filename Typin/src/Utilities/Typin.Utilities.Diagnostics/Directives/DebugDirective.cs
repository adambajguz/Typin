namespace Typin.Utilities.Diagnostics.Directives
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Models;
    using Typin.Models.Builders;
    using Typin.Schemas.Builders;

    /// <summary>
    /// When application runs in debug mode (using the [debug] directive), it will wait for debugger to be attached before proceeding.
    /// This is useful for debugging apps that were ran outside of the IDE.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class DebugDirective : IDirective //TODO: add directive hadnler
    {
        private sealed class Configure : IConfigureModel<DebugDirective>, IConfigureDirective<DebugDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<DebugDirective> builder, CancellationToken cancellationToken)
            {
                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<DebugDirective> builder, CancellationToken cancellationToken)
            {
                builder.AddAlias(DiagnosticsDirectives.Debug)
                    .UseDescription("Starts a debugging mode. Application will wait for debugger to be attached before proceeding.")
                    .UseHandler<Handler>();

                return default;
            }
        }

        private sealed class Handler : IDirectiveHandler<DebugDirective>
        {
            private readonly IConsole _console;

            /// <summary>
            /// Initializes a new instance of <see cref="DebugDirective"/>.
            /// </summary>
            public Handler(IConsole console)
            {
                _console = console;
            }

            /// <inheritdoc/>
            public async ValueTask ExecuteAsync(DirectiveArgs<DebugDirective> args, StepDelegate next, CancellationToken cancellationToken = default)
            {
#if NET5_0_OR_GREATER
                int processId = Environment.ProcessId;
#else
                int processId = Process.GetCurrentProcess().Id;
#endif

                _console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine($"Attach debugger to PID {processId} to continue."));

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

            public ValueTask ExecuteAsync(DebugDirective directive, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
