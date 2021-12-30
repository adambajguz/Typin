namespace Typin.Commands.Features
{
    using System.Collections.Generic;
    using Typin.Commands;
    using Typin.Commands.Schemas;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command feature.
    /// </summary>
    public interface ICommandFeature
    {
        /// <summary>
        /// Current command schema.
        /// </summary>
        ICommandSchema Schema { get; }

        /// <summary>
        /// Current command instance.
        /// </summary>
        ICommand Instance { get; }

        /// <summary>
        /// Current command handler instance.
        /// </summary>
        ICommandHandler HandlerInstance { get; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        IReadOnlyDictionary<IArgumentSchema, object?> DefaultValues { get; }
    }
}
