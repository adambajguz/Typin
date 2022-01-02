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
        /// Application description with markdown formatting.
        /// </summary>
        public string[]? Description { get; init; }

        /// <summary>
        /// Subheading with markdown formatting.
        /// </summary>
        public string DescriptionText => Description is null ? string.Empty : string.Join("\n", Description);

        /// <summary>
        /// Toast duration in milliseconds. Minimum duartion is 2ms, while default is 5ms.
        /// </summary>
        public int ToastDuration { get; init; }
    }
}
