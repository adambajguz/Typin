﻿namespace Typin.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    /// <summary>
    /// Normally when application runs in interactive mode, an empty line does nothing; but [!] will override this behaviour, executing a root or scoped command.
    /// This directive will also force default command execution when input contains default commmand parameter values equal to command/subcommand name.
    /// </summary>
    [Directive(BuiltInDirectives.Default, Description = "Executes a root or scoped command.")]
    public sealed class DefaultDirective : IDirective
    {
        /// <inheritdoc/>
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
