namespace TypinExamples.Configurations
{
    public sealed class ApplicationConfiguration
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Application logo path.
        /// </summary>
        public string? LogoPath { get; init; }

        /// <summary>
        /// Toast duration in milliseconds. Minimum duartion is 2ms, while default is 5ms.
        /// </summary>
        public int ToastDuration { get; init; }
    }
}
