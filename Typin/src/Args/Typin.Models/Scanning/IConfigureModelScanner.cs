namespace Typin.Models.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Hosting.Scanning;
    using Typin.Models.Builders;

    /// <summary>
    /// <see cref="IConfigureModel"/> component scanner.
    ///
    /// By default only types implementing <see cref="IConfigureModel{TModel}"/> are added when using From*.
    /// To add <see cref="IConfigureModel"/> use <see cref="IScanner{IConfigureModel}.Single(Type)"/> or <see cref="IScanner{IConfigureModel}.Multiple(IEnumerable{Type})"/>.
    /// </summary>
    public interface IConfigureModelScanner : IScanner<IConfigureModel>
    {
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureModelScanner Single(Func<IServiceProvider, IModelBuilder, CancellationToken, ValueTask> configure);

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureModelScanner Single<TModel>(Func<IServiceProvider, IModelBuilder<TModel>, CancellationToken, ValueTask> configure)
            where TModel : class, IModel;

        /// <summary>
        /// Adds <see cref="IConfigureModel"/> from nested classes in models.
        /// </summary>
        IConfigureModelScanner FromNested(Type type);
    }
}