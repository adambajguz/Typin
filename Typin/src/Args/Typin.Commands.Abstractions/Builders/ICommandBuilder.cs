namespace Typin.Commands.Builders
{
    using Typin.Commands;

    /// <summary>
    /// Command builder.
    /// </summary>
    public interface ICommandBuilder : IBaseCommandBuilder<ICommandBuilder>
    {

    }

    /// <summary>
    /// Command builder.
    /// </summary>
    public interface ICommandBuilder<TModel> : IBaseCommandBuilder<ICommandBuilder<TModel>>
        where TModel : class, ICommand
    {
        /// <summary>
        /// Configures a command handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        new ICommandBuilder<TModel> UseHandler<THandler>()
            where THandler : class, ICommandHandler<TModel>;
    }
}