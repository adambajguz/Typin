namespace Typin.Modes
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<int> ExecuteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid CLI mode.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICliMode)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
