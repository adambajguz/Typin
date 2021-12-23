namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Models.Schemas;
    using Typin.Schemas;

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
        /// Current command handler instance.
        /// </summary>
        ICommandHandler HandlerInstance { get; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        IReadOnlyDictionary<IArgumentSchema, object?> DefaultValues { get; }
    }
}
