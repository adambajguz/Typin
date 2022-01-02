namespace Typin.Directives.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives.Builders;
    using Typin.Directives.Internal;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureDirective"/> component scanner.
    /// </summary>
    internal sealed class ConfigureDirectiveScanner : Scanner<IConfigureDirective>, IConfigureDirectiveScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigureDirectiveScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public ConfigureDirectiveScanner(IServiceCollection services, IEnumerable<Type>? current) :
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
                            @interface.GetGenericTypeDefinition() == typeof(IConfigureDirective<>))
                        {
                            _services.AddTransient(typeof(IConfigureDirective<>), e.Type);
                        }
                    }
                }
                else if (!e.Type.IsGenericType)
                {
                    foreach (Type @interface in interfaces)
                    {
                        if (@interface == typeof(IConfigureDirective))
                        {
                            _services.AddTransient(@interface, e.Type);
                        }
                        else if (@interface.IsGenericType &&
                                 @interface.GetGenericTypeDefinition() == typeof(IConfigureDirective<>))
                        {
                            _services.AddTransient(@interface, e.Type);
                        }
                    }
                }
            };
        }

        /// <inheritdoc/>
        public override bool IsValid(Type type)
        {
            return IConfigureDirective.IsValidType(type) || IConfigureDirective.IsValidGenericType(type);
        }

        protected override IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.ExportedTypes
                .SelectMany(x => x
                    .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IConfigureDirective.IsValidGenericType)
                )
                .Concat(assembly.ExportedTypes.Where(IConfigureDirective.IsValidGenericType));
        }

        /// <inheritdoc/>
        public IConfigureDirectiveScanner Single(Func<IServiceProvider, IDirectiveBuilder, CancellationToken, ValueTask> configure)
        {
            _services.AddTransient<IConfigureDirective>(provider =>
            {
                return new InlineConfigureDirectives(provider, configure);
            });

            return this;
        }

        /// <inheritdoc/>
        public IConfigureDirectiveScanner Single<TDirective>(Func<IServiceProvider, IDirectiveBuilder<TDirective>, CancellationToken, ValueTask> configure)
            where TDirective : class, IDirective
        {
            _services.AddTransient<IConfigureDirective<TDirective>>(provider =>
            {
                return new InlineConfigureDirective<TDirective>(provider, configure);
            });

            return this;
        }

        /// <inheritdoc/>
        public IConfigureDirectiveScanner FromNested(Type type)
        {
            IEnumerable<Type>? types = type
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(IConfigureDirective.IsValidGenericType);

            if (types is not null)
            {
                Multiple(types);
            }

            return this;
        }
    }
}
