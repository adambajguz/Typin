namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands.Builders;
    using Typin.Commands.Internal;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureCommand"/> component scanner.
    /// </summary>
    internal sealed class ConfigureCommandScanner : Scanner<IConfigureCommand>, IConfigureCommandScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigureCommandScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public ConfigureCommandScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                Type[] interfaces = e.Type.GetInterfaces();

                if (e.Type.IsGenericType)
                {
                    foreach (Type @interface in interfaces)
                    {
                        if (@interface.IsGenericType &&
                            @interface.GetGenericTypeDefinition() == typeof(IConfigureCommand<>))
                        {
                            _services.AddTransient(typeof(IConfigureCommand<>), e.Type);
                        }
                    }
                }
                else if (!e.Type.IsGenericType)
                {
                    foreach (Type @interface in interfaces)
                    {
                        if (@interface == typeof(IConfigureCommand))
                        {
                            _services.AddTransient(@interface, e.Type);
                        }
                        else if (@interface.IsGenericType &&
                                 @interface.GetGenericTypeDefinition() == typeof(IConfigureCommand<>))
                        {
                            _services.AddTransient(@interface, e.Type);
                        }
                    }
                }
            };
        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return IConfigureCommand.IsValidType(type);
        }

        protected override IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.ExportedTypes
                .SelectMany(x => x
                    .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IsValidComponent)
                )
                .Concat(assembly.ExportedTypes.Where(IsValidComponent));
        }

        /// <inheritdoc/>
        public IConfigureCommandScanner Single(Func<IServiceProvider, ICommandBuilder, CancellationToken, ValueTask> configure)
        {
            _services.AddTransient<IConfigureCommand>(provider =>
            {
                return new InlineConfigureCommand(provider, configure);
            });

            return this;
        }

        /// <inheritdoc/>
        public IConfigureCommandScanner Single<TCommand>(Func<IServiceProvider, ICommandBuilder<TCommand>, CancellationToken, ValueTask> configure)
            where TCommand : class, ICommand
        {
            _services.AddTransient<IConfigureCommand<TCommand>>(provider =>
            {
                return new InlineConfigureCommand<TCommand>(provider, configure);
            });

            return this;
        }

        /// <inheritdoc/>
        public IConfigureCommandScanner FromNested(Type type)
        {
            IEnumerable<Type>? types = type
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(IsValidComponent);

            if (types is not null)
            {
                Multiple(types);
            }

            return this;
        }
    }
}
