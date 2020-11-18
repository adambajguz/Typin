namespace TypinExamples.TypinWeb.Logging
{
    using Microsoft.Extensions.Logging.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Configures a FileLoggerOptions instance by using ConfigurationBinder.Bind against an IConfiguration.
    /// <para>This class essentially binds a WebLoggerOptions instance with a section in the appsettings.json file.</para>
    /// </summary>
    internal class WebLoggerOptionsSetup : ConfigureFromConfigurationOptions<WebLoggerOptions>
    {
        /// <summary>
        /// Constructor that takes the IConfiguration instance to bind against.
        /// </summary>
        public WebLoggerOptionsSetup(ILoggerProviderConfiguration<WebLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {

        }
    }
}