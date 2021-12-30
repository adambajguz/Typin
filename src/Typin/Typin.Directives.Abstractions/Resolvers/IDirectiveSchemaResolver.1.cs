namespace Typin.Directives.Resolvers
{
    using System;

    /// <summary>
    /// <typeparamref name="TDirective"/> schema resolver.
    /// </summary>
    /// <typeparam name="TDirective"></typeparam>
    public interface IDirectiveSchemaResolver<TDirective> : IDirectiveSchemaResolver
        where TDirective : class, IDirective
    {
        Type IDirectiveSchemaResolver.DirectiveType => typeof(TDirective);
    }
}
