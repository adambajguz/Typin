namespace Typin.Plugins.Scopes.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Plugins.Scopes;

    /// <summary>
    /// If application runs in interactive mode, this [..] directive can be used to reset current scope to default (global scope).
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [..]
    ///             >
    /// </example>
    /// </summary>
    [Directive(ScopesDirectives.ScopeReset, Description = "Resets the scope to default value.")]
    public sealed class ScopeResetDirective : IDirective //TODO: add directive hadnler
    {
        private sealed class Handler : IDirectiveHandler<ScopeResetDirective>
        {
            private readonly IScopeManager _scopeManager;

            /// <summary>
            /// Initializes an instance of <see cref="ScopeResetDirective"/>.
            /// </summary>
            public Handler(IScopeManager scopeManager)
            {
                _scopeManager = scopeManager;
            }

            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<ScopeResetDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                _scopeManager.Reset();

                args.Context.Output.ExitCode ??= ExitCode.Success;

                return default;
            }
        }
    }
}
