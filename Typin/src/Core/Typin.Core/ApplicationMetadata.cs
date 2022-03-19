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
        /// Application executable name.
        /// </summary>
        public string? ExecutableName { get; set; }

        /// <summary>
        /// Application version text.
        /// </summary>
        public string? VersionText { get; set; }

        /// <summary>
        /// Application description.
        /// </summary>
        public string? Description { get; set; }
    }
}