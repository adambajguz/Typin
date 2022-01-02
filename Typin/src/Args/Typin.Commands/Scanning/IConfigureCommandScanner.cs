namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Builders;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureCommand"/> component scanner.
    ///
    /// By default only types implementing <see cref="IConfigureCommand{TModel}"/> are added when using From*.
    /// To add <see cref="IConfigureCommand"/> use <see cref="IScanner{IConfigureCommand}.Single(Type)"/> or <see cref="IScanner{IConfigureCommand}.Multiple(IEnumerable{Type})"/>.
    /// </summary>
    public interface IConfigureCommandScanner : IScanner<IConfigureCommand>
    {
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureCommandScanner Single(Func<IServiceProvider, ICommandBuilder, CancellationToken, ValueTask> configure);

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