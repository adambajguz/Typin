namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    /// <summary>
    /// Normally if application rans in interactive mode, an empty line does nothing; but [default] will override this behaviour, executing a root (empty) command or scoped command without arguments.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Directive("default", Description = "Executes a root (empty) command or scoped command without arguments (parameters and options).", InteractiveModeOnly = true)]
    public sealed class DefaultDirective : IDirective
    {
        /// <inheritdoc/>
        public bool ContinueExecution => true;

        /// <summary>
        /// Initializes an instance of <see cref="DefaultDirective"/>.
        /// </summary>
        public DefaultDirective()
        {

        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            //TODO: maybe make [default] -h etc forbidden
            //bool isInteractiveMode = _cliContext.IsInteractiveMode;
            //string scope = _cliContext.Scope;
            //CommandInput input = _cliContext.CurrentInput;

            // if (input.IsDefaultCommandOrEmpty)
            //ContinueExecution = true;

            return default;
        }
    }
}
