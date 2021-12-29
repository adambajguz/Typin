namespace Typin.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands.Builders;

    /// <summary>
    /// Represents an object that configures all commands.
    /// </summary>
    public interface IConfigureCommand
    {
        /// <summary>
        /// Configure model using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(ICommandBuilder builder, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid command configurator.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            Type[] interfaces = type.GetInterfaces();

            return (interfaces.Contains(typeof(IConfigureCommand)) ||
                    interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureCommand<>))) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid command configurator.
        /// </summary>
        public static bool IsValidType(Type type, Type commandType)
        {
            return type.GetInterfaces()
                .Contains(typeof(IConfigureCommand<>).MakeGenericType(commandType)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Represents an object that configures a command <typeparamref name="TCommand"/>.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface IConfigureCommand<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Configure model using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(ICommandBuilder<TCommand> builder, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid command configurator.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IConfigureCommand<TCommand>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
