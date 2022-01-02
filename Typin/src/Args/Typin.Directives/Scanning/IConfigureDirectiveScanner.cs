namespace Typin.Directives.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureDirective"/> component scanner.
    ///
    /// By default only types implementing <see cref="IConfigureDirective{TModel}"/> are added when using From*.
    /// To add <see cref="IConfigureDirective"/> use <see cref="IScanner{IConfigureDirective}.Single(Type)"/> or <see cref="IScanner{IConfigureDirective}.Multiple(IEnumerable{Type})"/>.
    /// </summary>
    public interface IConfigureDirectiveScanner : IScanner<IConfigureDirective>
    {
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureDirectiveScanner Single(Func<IServiceProvider, IDirectiveBuilder, CancellationToken, ValueTask> configure);

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/>.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureDirectiveScanner Single<TDirective>(Func<IServiceProvider, IDirectiveBuilder<TDirective>, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective;

        /// <summary>
        /// Adds <see cref="IConfigureDirective"/> from nested classes.
        /// </summary>
        IConfigureDirectiveScanner FromNested(Type type);
    }
}