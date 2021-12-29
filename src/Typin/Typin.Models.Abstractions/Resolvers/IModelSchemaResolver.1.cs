namespace Typin.Models.Resolvers
{
    using System;

    /// <summary>
    /// <typeparamref name="TModel"/> schema resolver.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IModelSchemaResolver<TModel> : IModelSchemaResolver
        where TModel : class, IModel
    {
        Type IModelSchemaResolver.ModelType => typeof(TModel);
    }
}
