namespace Typin.Commands.Resolvers
{
    using System;

    /// <summary>
    /// <typeparamref name="TCommand"/> schema resolver.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandSchemaResolver<TCommand> : ICommandSchemaResolver
        where TCommand : class, ICommand
    {
        Type ICommandSchemaResolver.CommandType => typeof(TCommand);
    }
}
