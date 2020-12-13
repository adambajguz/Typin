namespace TypinExamples.Configuration
{
    public sealed class LinkDefinition
    {
        /// <summary>
        /// Target path or URL.
        /// </summary>
        public string? Href { get; init; }

        /// <summary>
        /// Link title.
        /// </summary>
        public string? Title { get; init; }


        /// <summary>
        /// Whether link title shoudl be interpreted as html formatted title.
        /// </summary>
        public bool IsHtmlTitle { get; init; }

        /// <summary>
        /// Whether link is external.
        /// </summary>
        public bool IsExternal { get; init; }
    }
}
