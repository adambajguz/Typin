namespace Typin.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives.Builders;

    /// <summary>
    /// Represents an object that configures all directives.
    /// </summary>
    public interface IConfigureDirective
    {
        /// <summary>
        /// Configure directive using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(IDirectiveBuilder builder, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid directive configurator.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IConfigureDirective)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid directive configurator.
        /// </summary>
        public static bool IsValidGenericType(Type type)
        {
            Type[] interfaces = type.GetInterfaces();

            return interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureDirective<>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Represents an object that configures a directive <typeparamref name="TDirective"/>.
    /// </summary>
    /// <typeparam name="TDirective"></typeparam>
    public interface IConfigureDirective<TDirective>
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Configure directive using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(IDirectiveBuilder<TDirective> builder, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid directive configurator.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IConfigureDirective<TDirective>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
