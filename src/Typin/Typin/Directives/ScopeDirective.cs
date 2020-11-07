namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Internal.Extensions;
    using Typin.Modes;

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
    [Directive(BuiltInDirectives.Scope, Description = "Sets a scope to command(s).", SupportedModes = new[] { typeof(InteractiveMode) })]
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
            string? name = _cliContext.Input.CommandName ?? GetFallbackCommandName();

            if (name != null)
                _cliContext.Scope = name;

            return default;
        }

        private string? GetFallbackCommandName()
        {
            string potentialName = _cliContext.Input.Arguments.Skip(_cliContext.Input.Directives.Count)
                                                              .JoinToString(' ');

            if (_cliContext.RootSchema.IsCommandOrSubcommandPart(potentialName))
                return potentialName;

            return null;
        }
    }
}
