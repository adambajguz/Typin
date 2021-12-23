namespace Typin.GlobalArguments
{
    using Typin.Hosting;

    /// <summary>
    /// <see cref="ICliBuilder"/> command line extensions.
    /// </summary>
    public static class CliBuilderGlobalArgumentsExtensions
    {
        /// <summary>
        /// Sets <see cref="CliOptions.CommandLine"/> (resets <see cref="CliOptions.CommandLineArguments"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICliBuilder AddGlobalArguments(this ICliBuilder builder)
        {
            return builder;
        }
    }
}
