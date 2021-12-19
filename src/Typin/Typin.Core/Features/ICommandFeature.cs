namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Schemas;

    /// <summary>
    /// Command feature.
    /// </summary>
    public interface ICommandFeature
    {
        /// <summary>
        /// Current command schema.
        /// </summary>
        CommandSchema Schema { get; }

        /// <summary>
        /// Current command instance.
        /// </summary>
        ICommand Instance { get; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        IReadOnlyDictionary<ArgumentSchema, object?> DefaultValues { get; }
    }
}
