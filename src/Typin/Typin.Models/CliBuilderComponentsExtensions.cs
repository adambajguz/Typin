namespace Typin.Models
{
    using System;
    using Typin.Hosting;
    using Typin.Models.Scanning;

    /// <summary>
    /// <see cref="ICliBuilder"/> components extensions.
    /// </summary>
    public static class CliBuilderComponentsExtensions
    {
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
                    return new ConfigureModelScanner(cli.Services, current);
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
    }
}