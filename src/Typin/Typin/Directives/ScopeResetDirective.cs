﻿namespace Typin.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode, this [..] directive can be used to reset current scope to default (global scope).
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [..]
    ///             >
    /// </example>
    /// </summary>
    [Directive(BuiltInDirectives.ScopeReset, Description = "Resets the scope to default value.", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class ScopeResetDirective : IPipelinedDirective
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
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _options.Scope = string.Empty;
            args.ExitCode ??= ExitCodes.Success;

            return default;
        }
    }
}
