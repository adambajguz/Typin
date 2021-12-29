namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Modes;

    /// <summary>
    /// <see cref="ICliBuilder"/> startup mode extensions.
    /// </summary>
    public static class CliBuilderStartupModeExtensions
    {
        /// <summary>
        /// Sets a startup mode.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="modeType"></param>
        /// <returns></returns>
        public static ICliBuilder SetStartupMode(this ICliBuilder builder, Type modeType)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.StartupMode = modeType;
            });

            return builder;
        }

        /// <summary>
        /// Sets a startup mode.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICliBuilder SetStartupMode<T>(this ICliBuilder builder)
            where T : class, ICliMode
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.StartupMode = typeof(T);
            });

            return builder;
        }
    }
}
