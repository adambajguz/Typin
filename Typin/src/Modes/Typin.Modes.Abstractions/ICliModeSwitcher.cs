namespace Typin.Modes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI mode switcher.
    /// </summary>
    public interface ICliModeSwitcher
    {
        /// <summary>
        /// Switches mode to <typeparamref name="TMode"/>.
        /// </summary>
        /// <typeparam name="TMode"></typeparam>
        /// <param name="delegate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WithModeAsync<TMode>(Func<TMode, CancellationToken, Task> @delegate, CancellationToken cancellationToken = default)
            where TMode : class, ICliMode;

        /// <summary>
        /// Switches mode to <paramref name="modeType"/>.
        /// </summary>
        /// <param name="modeType"></param>
        /// <param name="delegate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WithModeAsync(Type modeType, Func<ICliMode, CancellationToken, Task> @delegate, CancellationToken cancellationToken = default);
    }
}