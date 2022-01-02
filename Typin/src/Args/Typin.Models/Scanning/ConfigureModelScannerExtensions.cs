namespace Typin.Models.Scanning
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Models.Builders;

    /// <summary>
    /// <see cref="IConfigureModelScanner"/> extensions.
    /// </summary>
    public static class ConfigureModelScannerExtensions
    {
        #region NonGeneric
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single(this IConfigureModelScanner scanner,
                                                    Action<IModelBuilder> configure)
        {
            return scanner.Single((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline global configuration with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TDep0>(this IConfigureModelScanner scanner,
                                                           Func<IModelBuilder, TDep0, CancellationToken, ValueTask> configure)
            where TDep0 : class
        {
            return scanner.Single(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();

                await configure(builder, dep0, ct);
            });
        }

        /// <summary>
        /// Adds an inline global configuration with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TDep0, TDep1>(this IConfigureModelScanner scanner,
                                                                  Func<IModelBuilder, TDep0, TDep1, CancellationToken, ValueTask> configure)
            where TDep0 : class
            where TDep1 : class
        {
            return scanner.Single(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();

                await configure(builder, dep0, dep1, ct);
            });
        }

        /// <summary>
        /// Adds an inline global configuration with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <typeparam name="TDep2"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TDep0, TDep1, TDep2>(this IConfigureModelScanner scanner,
                                                                         Func<IModelBuilder, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
            where TDep0 : class
            where TDep1 : class
            where TDep2 : class
        {
            return scanner.Single(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                await configure(builder, dep0, dep1, dep2, ct);
            });
        }

        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single(this IConfigureModelScanner scanner,
                                                    Func<IModelBuilder, CancellationToken, ValueTask> configure)
        {
            return scanner.Single(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion

        #region Generic
        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TModel>(this IConfigureModelScanner scanner,
                                                            Action<IModelBuilder<TModel>> configure)
            where TModel : class, IModel
        {
            return scanner.Single<TModel>((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/> with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TModel, TDep0>(this IConfigureModelScanner scanner,
                                                                   Func<IModelBuilder<TModel>, TDep0, CancellationToken, ValueTask> configure)
            where TModel : class, IModel
            where TDep0 : class
        {
            return scanner.Single<TModel>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();

                await configure(builder, dep0, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TModel, TDep0, TDep1>(this IConfigureModelScanner scanner,
                                                                          Func<IModelBuilder<TModel>, TDep0, TDep1, CancellationToken, ValueTask> configure)
            where TModel : class, IModel
            where TDep0 : class
            where TDep1 : class
        {
            return scanner.Single<TModel>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();

                await configure(builder, dep0, dep1, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <typeparam name="TDep2"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TModel, TDep0, TDep1, TDep2>(this IConfigureModelScanner scanner,
                                                                                 Func<IModelBuilder<TModel>, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
            where TModel : class, IModel
            where TDep0 : class
            where TDep1 : class
            where TDep2 : class
        {
            return scanner.Single<TModel>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                await configure(builder, dep0, dep1, dep2, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureModelScanner Single<TModel>(this IConfigureModelScanner scanner,
                                                            Func<IModelBuilder<TModel>, CancellationToken, ValueTask> configure)
            where TModel : class, IModel
        {
            return scanner.Single<TModel>(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion
    }
}
