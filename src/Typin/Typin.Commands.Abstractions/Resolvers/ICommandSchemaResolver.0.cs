namespace Typin.Commands.Resolvers
{
    using System;
    using Typin.Commands.Schemas;
    using Typin.Models.Builders;

    /// <summary>
    /// Command schema resolver.
    /// </summary>
    public interface ICommandSchemaResolver : IAsyncResolver<ICommandSchema>
    {
        /// <summary>
        /// Command type to resolve.
        /// </summary>
        Type CommandType { get; }
    }
}
