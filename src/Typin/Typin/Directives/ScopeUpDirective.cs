namespace Typin.Directives
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
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
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.ScopeUp, Description = "Removes one command from the scope.", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class ScopeUpDirective : IDirective
    {
        private readonly InteractiveModeSettings _settings;

        /// <inheritdoc/>
        public bool ContinueExecution => false;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeUpDirective"/>.
        /// </summary>
        public ScopeUpDirective(InteractiveModeSettings interactiveModeSettings)
        {
            _settings = interactiveModeSettings;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            // Scope up
            string[] splittedScope = _settings.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splittedScope.Length > 1)
                _settings.Scope = string.Join(" ", splittedScope, 0, splittedScope.Length - 1);
            else if (splittedScope.Length == 1)
                _settings.Scope = string.Empty;

            return default;
        }
    }
}
