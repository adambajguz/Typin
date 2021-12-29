namespace Typin.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Entry point for a <see cref="IDirective"/>.
    /// </summary>
    public interface IDirectiveHandler
    {
        /// <summary>
        /// Checks whether type is a valid directive handler.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            Type[] interfaces = type.GetInterfaces();

            return interfaces.Contains(typeof(IDirectiveHandler)) &&
                interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDirectiveHandler<>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid directive handler.
        /// </summary>
        public static bool IsValidType(Type type, Type directiveType)
        {
            return type.GetInterfaces().Contains(typeof(IDirectiveHandler<>).MakeGenericType(directiveType)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Entry point for a <see cref="IDirective"/>.
    /// </summary>
    public interface IDirectiveHandler<TDirective> : IDirectiveHandler
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Executes the directive.
        /// This is the method that's called when the directive is invoked by a user through directive line.
        /// </summary>
        /// <remarks>If the execution of the directive is not asynchronous, simply end the method with <code>return default;</code></remarks>
        ValueTask ExecuteAsync(TDirective directive, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid directive handler.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IDirectiveHandler<TDirective>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}