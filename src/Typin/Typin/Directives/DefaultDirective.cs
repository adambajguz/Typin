namespace Typin.Directives
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    /// <summary>
    /// Normally when application runs in interactive mode, an empty line does nothing; but [!] will override this behaviour, executing a root or scoped command.
    /// This directive will also force defualt command execution when input contains default commmand parmameter values equal to command/subcommand name.
    /// </summary>
    [Directive(BuiltInDirectives.Default, Description = "Executes a root or scoped command.")]
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
