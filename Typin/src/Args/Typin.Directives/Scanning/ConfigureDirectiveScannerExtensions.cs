namespace Typin.Directives.Scanning
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives.Builders;

    /// <summary>
    /// <see cref="IConfigureDirectiveScanner"/> extensions.
    /// </summary>
    public static class ConfigureDirectiveScannerExtensions
    {
        #region NonGeneric
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single(this IConfigureDirectiveScanner scanner,
                                                      Action<IDirectiveBuilder> configure)
        {
            return scanner.Single((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline global configuration for <typeparamref name="TDirective"/> with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective, TDep0>(this IConfigureDirectiveScanner scanner,
                                                                       Func<IDirectiveBuilder, TDep0, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
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
        public static IConfigureDirectiveScanner Single<TDep0, TDep1>(this IConfigureDirectiveScanner scanner,
                                                                    Func<IDirectiveBuilder, TDep0, TDep1, CancellationToken, ValueTask> configure)
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
        public static IConfigureDirectiveScanner Single<TDep0, TDep1, TDep2>(this IConfigureDirectiveScanner scanner,
                                                                           Func<IDirectiveBuilder, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
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
        public static IConfigureDirectiveScanner Single<TDirective>(this IConfigureDirectiveScanner scanner,
                                                                Func<IDirectiveBuilder, CancellationToken, ValueTask> configure)
        {
            return scanner.Single(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion

        #region Generic
        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/>.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective>(this IConfigureDirectiveScanner scanner,
                                                                Action<IDirectiveBuilder<TDirective>> configure)
            where TDirective : class, IDirective
        {
            return scanner.Single<TDirective>((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/> with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective, TDep0>(this IConfigureDirectiveScanner scanner,
                                                                       Func<IDirectiveBuilder<TDirective>, TDep0, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
            where TDep0 : class
        {
            return scanner.Single<TDirective>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();

                await configure(builder, dep0, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective, TDep0, TDep1>(this IConfigureDirectiveScanner scanner,
                                                                              Func<IDirectiveBuilder<TDirective>, TDep0, TDep1, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
            where TDep0 : class
            where TDep1 : class
        {
            return scanner.Single<TDirective>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();

                await configure(builder, dep0, dep1, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <typeparam name="TDep2"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective, TDep0, TDep1, TDep2>(this IConfigureDirectiveScanner scanner,
                                                                                     Func<IDirectiveBuilder<TDirective>, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
            where TDep0 : class
            where TDep1 : class
            where TDep2 : class
        {
            return scanner.Single<TDirective>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                await configure(builder, dep0, dep1, dep2, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TDirective"/>.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner Single<TDirective>(this IConfigureDirectiveScanner scanner,
                                                                Func<IDirectiveBuilder<TDirective>, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
        {
            return scanner.Single<TDirective>(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion
    }
}
