namespace Typin.Plugins.Scopes.Directives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Plugins.Scopes;

    /// <summary>
    /// If application runs in interactive mode, [>] directive followed by command(s) would scope to the command(s), allowing to ommit specified command name(s).
    /// <example>
    /// Commands:
    ///              > [>] cmd1 sub
    ///      cmd1 sub> list
    ///      cmd1 sub> get
    ///              > [>] cmd1
    ///          cmd1> test
    ///          cmd1> -h
    ///
    /// are an equivalent to:
    ///              > cmd1 sub list
    ///              > cmd1 sub get
    ///              > cmd1 test
    ///              > cmd1 -h
    /// </example>
    /// </summary>
    [Directive(ScopesDirectives.Scope, Description = "Sets a scope to command(s).")]
    public sealed class ScopeDirective : IDirective
    {
        private sealed class Handler : IDirectiveHandler<ScopeDirective>
        {
            private readonly IScopeManager _scopeManager;

            /// <summary>
            /// Initializes an instance of <see cref="ScopeDirective"/>.
            /// </summary>
            public Handler(IScopeManager scopeManager)
            {
                _scopeManager = scopeManager;
            }

            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<ScopeDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                CliContext context = args.Context;
                string? name = context.Input.Tokens?.CommandName ?? throw new NullReferenceException("Input not parsed."); ;

                if (name is not null)
                {
                    _scopeManager.Set(name);
                    context.Output.ExitCode ??= ExitCode.Success;
                }

                return default;
            }
        }
    }
}
