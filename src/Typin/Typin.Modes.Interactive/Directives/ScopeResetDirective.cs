namespace Typin.Modes.Interactive.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Directives;
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
    [Directive(InteractiveOnlyDirectives.ScopeReset, Description = "Resets the scope to default value.", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class ScopeResetDirective : IDirective //TODO: add directive hadnler
    {
        private readonly InteractiveModeOptions _options;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeResetDirective"/>.
        /// </summary>
        public ScopeResetDirective(IOptions<InteractiveModeOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _options.Scope = string.Empty;
            args.Output.ExitCode ??= ExitCode.Success;

            return default;
        }
    }
}
