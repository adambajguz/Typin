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
    using Typin.Models.Internal;

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
                Type[] interfaces = e.Type.GetInterfaces();

                if (e.Type.IsGenericType)
                {
                    foreach (Type @interface in interfaces)
                    {
                        if (@interface.IsGenericType &&
                            @interface.GetGenericTypeDefinition() == typeof(IConfigureModel<>))
                        {
                            _services.AddTransient(typeof(IConfigureModel<>), e.Type);
                        }
                    }
                }
                else if (!e.Type.IsGenericType)
                {
                    foreach (Type @interface in interfaces)
                    {
                        if (@interface == typeof(IConfigureModel))
                        {
                            _services.AddTransient(@interface, e.Type);
                        }
                        else if (@interface.IsGenericType &&
                                 @interface.GetGenericTypeDefinition() == typeof(IConfigureModel<>))
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
            return IConfigureModel.IsValidGenericType(type) || IConfigureModel.IsValidType(type);
        }

        protected override IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.ExportedTypes
                .SelectMany(x => x
                    .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IConfigureModel.IsValidGenericType)
                )
                .Concat(assembly.ExportedTypes.Where(IConfigureModel.IsValidGenericType));
        }

        /// <inheritdoc/>
        public IConfigureModelScanner Single(Func<IServiceProvider, IModelBuilder, CancellationToken, ValueTask> configure)
        {
            _services.AddTransient<IConfigureModel>(provider =>
            {
                return new InlineConfigureModels(provider, configure);
            });

            return this;
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
                .Where(IConfigureModel.IsValidGenericType);

            if (types is not null)
            {
                Multiple(types);
            }

            return this;
        }
    }
}
