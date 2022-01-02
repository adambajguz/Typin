namespace Typin.Help
{
    using System.Collections.Generic;
    using Typin.Commands.Schemas;
    using Typin.Models.Schemas;

    /// <summary>
    /// Help text writer.
    /// </summary>
    public interface IHelpWriter
    {
        /// <summary>
        /// Writes help for the current command in context.
        /// </summary>
        void Write();

        /// <summary>
        /// Writes help for a command.
        /// </summary>
        void Write(ICommandSchema command, IReadOnlyDictionary<IArgumentSchema, object?> defaultValues);
    }
}