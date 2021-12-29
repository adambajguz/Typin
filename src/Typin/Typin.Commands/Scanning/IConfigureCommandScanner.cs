namespace Typin.Commands.Scanning
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Builders;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureCommand"/> component scanner.
    /// </summary>
    public interface IConfigureCommandScanner : IScanner<IConfigureCommand>
    {
        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureCommandScanner Single<TCommand>(Func<IServiceProvider, ICommandBuilder<TCommand>, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand;

        /// <summary>
        /// Adds <see cref="IConfigureCommand"/> from nested classes.
        /// </summary>
        IConfigureCommandScanner FromNested(Type type);
    }
}