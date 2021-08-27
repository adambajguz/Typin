namespace Typin
{
    /// <summary>
    /// Metadata associated with the application.
    /// </summary>
    public sealed class ApplicationMetadata
    {
        /// <summary>
        /// Application title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Applicatin executable name.
        /// </summary>
        public string? ExecutableName { get; set; }

        /// <summary>
        /// Applicatin version text.
        /// </summary>
        public string? VersionText { get; set; }

        /// <summary>
        /// Application description.
        /// </summary>
        public string? Description { get; set; }
    }
}