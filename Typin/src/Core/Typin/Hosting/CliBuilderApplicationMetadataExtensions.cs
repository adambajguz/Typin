namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="ICliBuilder"/> application metadata extensions.
    /// </summary>
    public static class CliBuilderApplicationMetadataExtensions
    {
        /// <summary>
        /// Resets application metadata.
        /// </summary>
        public static ICliBuilder ResetApplicationMetadata(this ICliBuilder cliBuilder)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options =>
            {
                options.Title = null;
                options.VersionText = null;
                options.ExecutableName = null;
                options.Description = null;
            });

            return cliBuilder;
        }

        /// <summary>
        /// Sets application metadata.
        /// </summary>
        public static ICliBuilder ConfigureApplicationMetadata(this ICliBuilder cliBuilder, Action<ApplicationMetadata> options)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options);

            return cliBuilder;
        }

        /// <summary>
        /// Sets application title.
        /// </summary>
        public static ICliBuilder UseTitle(this ICliBuilder cliBuilder, string? title)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options => options.Title = title);

            return cliBuilder;
        }

        /// <summary>
        /// Sets application description.
        /// </summary>
        public static ICliBuilder UseDescription(this ICliBuilder cliBuilder, string? description)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options => options.Description = description);

            return cliBuilder;
        }

        /// <summary>
        /// Sets application executable name.
        /// </summary>
        public static ICliBuilder UseExecutableName(this ICliBuilder cliBuilder, string? executableName)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options => options.ExecutableName = executableName);

            return cliBuilder;
        }

        /// <summary>
        /// Sets application version text.
        /// </summary>
        public static ICliBuilder UseVersionText(this ICliBuilder cliBuilder, string? versionText)
        {
            cliBuilder.Services.Configure<ApplicationMetadata>(options => options.VersionText = versionText);

            return cliBuilder;
        }
    }
}
