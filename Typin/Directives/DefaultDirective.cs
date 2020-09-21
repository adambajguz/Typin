namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    /// <summary>
    /// Normally when application runs in interactive mode, an empty line does nothing; but [default] will override this behaviour, executing a root or scoped command.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.Default, Description = "Executes a root or scoped command.", InteractiveModeOnly = true)]
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
            return default;
        }
    }
}
