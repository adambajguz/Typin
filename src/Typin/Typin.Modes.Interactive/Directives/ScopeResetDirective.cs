namespace Typin.Modes.Interactive.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Modes.Interactive;

    /// <summary>
    /// If application runs in interactive mode, this [..] directive can be used to reset current scope to default (global scope).
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [..]
    ///             >
    /// </example>
    /// </summary>
    [Directive(InteractiveOnlyDirectives.ScopeReset, Description = "Resets the scope to default value.")]
    public sealed class ScopeResetDirective : IDirective //TODO: add directive hadnler
    {
        private sealed class Handler : IDirectiveHandler<ScopeResetDirective>
        {
            private readonly InteractiveModeOptions _options;

            /// <summary>
            /// Initializes an instance of <see cref="ScopeResetDirective"/>.
            /// </summary>
            public Handler(IOptions<InteractiveModeOptions> options)
            {
                _options = options.Value;
            }

            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<ScopeResetDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                _options.Scope = string.Empty;
                args.Context.Output.ExitCode ??= ExitCode.Success;

                return default;
            }
        }
    }
}
