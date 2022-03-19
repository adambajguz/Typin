namespace Typin.Commands.Scanning
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands.Builders;

    /// <summary>
    /// <see cref="IConfigureCommandScanner"/> extensions.
    /// </summary>
    public static class ConfigureCommandScannerExtensions
    {
        #region NonGeneric
        /// <summary>
        /// Adds an inline global configuration.
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single(this IConfigureCommandScanner scanner,
                                                      Action<ICommandBuilder> configure)
        {
            return scanner.Single((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline global configuration for <typeparamref name="TCommand"/> with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand, TDep0>(this IConfigureCommandScanner scanner,
                                                                       Func<ICommandBuilder, TDep0, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
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
        public static IConfigureCommandScanner Single<TDep0, TDep1>(this IConfigureCommandScanner scanner,
                                                                    Func<ICommandBuilder, TDep0, TDep1, CancellationToken, ValueTask> configure)
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
        public static IConfigureCommandScanner Single<TDep0, TDep1, TDep2>(this IConfigureCommandScanner scanner,
                                                                           Func<ICommandBuilder, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
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
        public static IConfigureCommandScanner Single<TCommand>(this IConfigureCommandScanner scanner,
                                                                Func<ICommandBuilder, CancellationToken, ValueTask> configure)
        {
            return scanner.Single(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion

        #region Generic
        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand>(this IConfigureCommandScanner scanner,
                                                                Action<ICommandBuilder<TCommand>> configure)
            where TCommand : class, ICommand
        {
            return scanner.Single<TCommand>((provider, builder, ct) =>
            {
                configure(builder);

                return default;
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/> with a dependency resolved from services container.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand, TDep0>(this IConfigureCommandScanner scanner,
                                                                       Func<ICommandBuilder<TCommand>, TDep0, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
            where TDep0 : class
        {
            return scanner.Single<TCommand>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();

                await configure(builder, dep0, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand, TDep0, TDep1>(this IConfigureCommandScanner scanner,
                                                                              Func<ICommandBuilder<TCommand>, TDep0, TDep1, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
            where TDep0 : class
            where TDep1 : class
        {
            return scanner.Single<TCommand>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();

                await configure(builder, dep0, dep1, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/> with a dependencies resolved from services container.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TDep0"></typeparam>
        /// <typeparam name="TDep1"></typeparam>
        /// <typeparam name="TDep2"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand, TDep0, TDep1, TDep2>(this IConfigureCommandScanner scanner,
                                                                                     Func<ICommandBuilder<TCommand>, TDep0, TDep1, TDep2, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
            where TDep0 : class
            where TDep1 : class
            where TDep2 : class
        {
            return scanner.Single<TCommand>(async (provider, builder, ct) =>
            {
                TDep0 dep0 = provider.GetRequiredService<TDep0>();
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                await configure(builder, dep0, dep1, dep2, ct);
            });
        }

        /// <summary>
        /// Adds an inline configuration for <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="scanner"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner Single<TCommand>(this IConfigureCommandScanner scanner,
                                                                Func<ICommandBuilder<TCommand>, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
        {
            return scanner.Single<TCommand>(async (provider, builder, ct) => await configure(builder, ct));
        }
        #endregion
    }
}
