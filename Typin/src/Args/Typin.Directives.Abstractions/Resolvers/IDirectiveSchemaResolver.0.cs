namespace Typin.Directives.Resolvers
{
    using System;
    using Typin.Directives.Schemas;
    using Typin.Models.Resolvers;

    /// <summary>
    /// Directive schema resolver.
    /// </summary>
    public interface IDirectiveSchemaResolver : IAsyncResolver<IDirectiveSchema>
    {
        /// <summary>
        /// Directive type to resolve.
        /// </summary>
        Type DirectiveType { get; }
    }
}
