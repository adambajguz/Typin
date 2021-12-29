namespace Typin.Models.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Scanning;
    using Typin.Models.Builders;

    /// <summary>
    /// <see cref="IConfigureModel"/> component scanner.
    /// </summary>
    internal sealed class ConfigureModelScanner : Scanner<IConfigureModel>, IConfigureModelScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigureModelScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public ConfigureModelScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                _services.AddTransient(e.Type);

                Type[] interfaces = e.Type.GetInterfaces();

                foreach (Type @interface in interfaces)
                {
                    if (@interface.IsGenericType &&
                        @interface.GetGenericTypeDefinition() == typeof(IConfigureModel<>))
                    {
                        _services.AddTransient(@interface, e.Type);
                    }
                }
            };
        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return IConfigureModel.IsValidType(type);
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
        public IConfigureModelScanner Single<TModel>(Func<IServiceProvider, IModelBuilder<TModel>, CancellationToken, ValueTask> configure)
            where TModel : class, IModel
        {
            _services.AddTransient<IConfigureModel<TModel>>(provider =>
            {
                return new InlineConfigureModel<TModel>(provider, configure);
            });

            return this;
        }

        /// <inheritdoc/>
        public IConfigureModelScanner FromNested(Type type)
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
