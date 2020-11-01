namespace TypinExamples.Configuration
{
    using System;

    public sealed class HeaderSettings
    {
        /// <summary>
        /// Heading with Markdown formatting.
        /// </summary>
        public string? Heading { get; set; }

        /// <summary>
        /// Subheading with markdown formatting.
        /// </summary>
        public string? Subheading { get; set; }

        /// <summary>
        /// Links colleciton.
        /// </summary>
        public HeaderLink[] Links { get; set; } = Array.Empty<HeaderLink>();
    }

    public sealed class HeaderLink
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
