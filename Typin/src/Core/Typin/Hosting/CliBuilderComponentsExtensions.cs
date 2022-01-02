namespace Typin.Hosting
{
    using System;
    using Typin.Modes;

    /// <summary>
    /// <see cref="ICliBuilder"/> components extensions.
    /// </summary>
    public static class CliBuilderComponentsExtensions
    {
        /// <summary>
        /// Adds modes to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static ICliModeScanner AddModes(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<ICliMode, ICliModeScanner>(
                (cli, current) =>
                {
                    return new CliModeScanner(cli.Services, current);
                });
        }

        /// <summary>
        /// Adds modes to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddModes(this ICliBuilder cliBuilder, Action<ICliModeScanner> scanner)
        {
            scanner(cliBuilder.AddModes());

            return cliBuilder;
        }
    }
}