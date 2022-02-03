namespace Typin.Console
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Named console descriptor.
    /// </summary>
    public sealed record NamedConsoleDescriptor
    {
        /// <summary>
        /// Console name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Console lifetime.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Implementation type.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="NamedConsoleDescriptor"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lifetime"></param>
        /// <param name="implementationType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public NamedConsoleDescriptor(string name, ServiceLifetime lifetime, Type implementationType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Lifetime = lifetime;
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));

            if (implementationType.IsAssignableFrom(typeof(IConsole)))
            {
                throw new ArgumentException($"'{implementationType}' does not implement {typeof(IConsole)}.");
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="NamedConsoleDescriptor"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static NamedConsoleDescriptor Create<T>(string name, ServiceLifetime lifetime)
            where T : class, IConsole
        {
            return new NamedConsoleDescriptor(name, lifetime, typeof(T));
        }
    }
}
