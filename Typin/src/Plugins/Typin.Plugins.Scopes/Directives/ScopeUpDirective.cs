namespace Typin.Plugins.Scopes.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Plugins.Scopes;

    /// <summary>
    /// If application runs in interactive mode, this [.] directive can be used to remove one command from the scope.
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [.]
    ///         cmd1>
    /// </example>
    /// </summary>
    [Directive(ScopesDirectives.ScopeUp, Description = "Removes one command from the scope.")]
    public sealed class ScopeUpDirective : IDirective //TODO: add directive hadnler
    {
        private sealed class Handler : IDirectiveHandler<ScopeResetDirective>
        {
            private readonly IScopeManager _scopeManager;

            /// <summary>
            /// Initializes an instance of <see cref="ScopeUpDirective"/>.
            /// </summary>
            public Handler(IScopeManager scopeManager)
            {
                _scopeManager = scopeManager;
            }

            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<ScopeResetDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                _scopeManager.Up();

                args.Context.Output.ExitCode ??= ExitCode.Success;

                return default;
            }
        }
    }
}
