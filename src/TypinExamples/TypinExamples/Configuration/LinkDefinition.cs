namespace TypinExamples.Configuration
{
    public sealed class LinkDefinition
    {
        /// <summary>
        /// Target path or URL.
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// Link title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Whether link is external.
        /// </summary>
        public bool IsExternal { get; set; }
    }
}
