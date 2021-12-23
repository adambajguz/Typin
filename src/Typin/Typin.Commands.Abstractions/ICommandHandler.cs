namespace Typin
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Entry point for a <see cref="ICommand"/>.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Executes the command.
        /// This is the method that's called when the command is invoked by a user through command line.
        /// </summary>
        /// <remarks>If the execution of the command is not asynchronous, simply end the method with <code>return default;</code></remarks>
        public ValueTask ExecuteAsync(object command, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid command handler.
        /// </summary>
        public static bool IsValidType(Type type, Type commandType)
        {
            return type.GetInterfaces().Contains(typeof(ICommandHandler<>).MakeGenericType(commandType)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Entry point for a <see cref="ICommand"/>.
    /// </summary>
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : class, ICommand
    {
        ValueTask ICommandHandler.ExecuteAsync(object command, CancellationToken cancellationToken)
        {
            return ExecuteAsync((TCommand)command, cancellationToken);
        }

        /// <summary>
        /// Executes the command.
        /// This is the method that's called when the command is invoked by a user through command line.
        /// </summary>
        /// <remarks>If the execution of the command is not asynchronous, simply end the method with <code>return default;</code></remarks>
        ValueTask ExecuteAsync(TCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid command handler.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICommandHandler<TCommand>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}