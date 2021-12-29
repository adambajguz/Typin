namespace Typin.Modes.Interactive.Directives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Modes.Interactive;

    /// <summary>
    /// If application runs in interactive mode, this [.] directive can be used to remove one command from the scope.
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [.]
    ///         cmd1>
    /// </example>
    /// </summary>
    [Directive(InteractiveOnlyDirectives.ScopeUp, Description = "Removes one command from the scope.")]
    public sealed class ScopeUpDirective : IDirective //TODO: add directive hadnler
    {
        private readonly InteractiveModeOptions _options;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeUpDirective"/>.
        /// </summary>
        public ScopeUpDirective(IOptions<InteractiveModeOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Scope up
            string[] splittedScope = _options.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splittedScope.Length > 1)
            {
                _options.Scope = string.Join(" ", splittedScope, 0, splittedScope.Length - 1);
            }
            else if (splittedScope.Length == 1)
            {
                _options.Scope = string.Empty;
            }

            args.Output.ExitCode ??= ExitCode.Success;

            return default;
        }
    }
}
