namespace Typin.Modes
{
    /// <summary>
    /// <see cref="CliApplicationBuilder"/> direct mode configuration extensions.
    /// </summary>
    public static class DirectModeBuilderExtensions
    {
        /// <summary>
        /// Adds a direct mode to the application.
        /// </summary>
        public static CliApplicationBuilder UseDirectMode(this CliApplicationBuilder builder,
                                                          bool asStartup = false)
        {
            builder.RegisterMode<DirectMode>(asStartup);

            return builder;
        }
    }
}
