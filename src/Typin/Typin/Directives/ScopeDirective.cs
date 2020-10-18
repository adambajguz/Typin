﻿namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

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
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.Scope, Description = "Sets a scope to command(s).", InteractiveModeOnly = true)]
    public sealed class ScopeDirective : IDirective
    {
        private readonly CliContext _cliContext;

        /// <inheritdoc/>
        public bool ContinueExecution => false;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeDirective"/>.
        /// </summary>
        public ScopeDirective(ICliContext cliContext)
        {
            _cliContext = (CliContext)cliContext;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            _cliContext.Scope = _cliContext.Input.CommandName ?? string.Empty;

            return default;
        }
    }
}
