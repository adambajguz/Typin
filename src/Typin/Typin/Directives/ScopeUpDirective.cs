namespace Typin.Directives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.Attributes;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode, this [.] directive can be used to remove one command from the scope.
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [.]
    ///         cmd1>
    /// </example>
    /// </summary>
    [Directive(BuiltInDirectives.ScopeUp, Description = "Removes one command from the scope.", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class ScopeUpDirective : IPipelinedDirective
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
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            // Scope up
            string[] splittedScope = _options.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splittedScope.Length > 1)
                _options.Scope = string.Join(" ", splittedScope, 0, splittedScope.Length - 1);
            else if (splittedScope.Length == 1)
                _options.Scope = string.Empty;

            context.ExitCode ??= ExitCodes.Success;

            return default;
        }
    }
}
