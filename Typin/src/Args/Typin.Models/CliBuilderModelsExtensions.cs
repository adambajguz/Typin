namespace Typin.Models
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;
    using Typin.Models.Collections;
    using Typin.Models.Resolvers;
    using Typin.Models.Scanning;
    using Typin.Models.Schemas;

    /// <summary>
    /// <see cref="ICliBuilder"/> models-related extensions.
    /// </summary>
    public static class CliBuilderModelsExtensions
    {
        #region AddModels
        /// <summary>
        /// Adds model configurators to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IModelScanner AddModels(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IModel, IModelScanner>(
                (cli, current) =>
                {
                    return new AggregatedModelScanner(cli, current);
                });
        }

        /// <summary>
        /// Adds model configurators to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddModels(this ICliBuilder cliBuilder, Action<IModelScanner> scanner)
        {
            scanner(cliBuilder.AddModels());

            return cliBuilder;
        }
        #endregion

        #region ConfigureModels
        /// <summary>
        /// Adds model configurators to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IConfigureModelScanner ConfigureModels(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IConfigureModel, IConfigureModelScanner>(
                (cli, current) =>
                {
                    IServiceCollection services = cli.Services;
                    if (!services.Any(x => x.ImplementationType == typeof(ModelSchemaProvider)))
                    {
                        services.AddSingleton<IModelSchemaCollection, ModelSchemaCollection>();
                        services.AddScoped<IModelSchemaProvider, ModelSchemaProvider>();
                        services.AddHostedService<ModelSchemaProviderHostedService>();
                        services.AddTransient(typeof(IModelSchemaResolver<>), typeof(ModelSchemaResolver<>));
                    }

                    return new ConfigureModelScanner(services, current);
                });
        }

        /// <summary>
        /// Adds model configurators to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder ConfigureModels(this ICliBuilder cliBuilder, Action<IConfigureModelScanner> scanner)
        {
            scanner(cliBuilder.ConfigureModels());

            return cliBuilder;
        }
        #endregion
    }
}