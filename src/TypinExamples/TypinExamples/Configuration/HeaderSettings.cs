namespace TypinExamples.Configuration
{
    using System;

    public sealed class HeaderSettings
    {
        /// <summary>
        /// Heading with Markdown formatting.
        /// </summary>
        public string? Heading { get; init; }

        /// <summary>
        /// Subheading with markdown formatting.
        /// </summary>
        public string? Subheading { get; init; }

        /// <summary>
        /// Links colleciton.
        /// </summary>
        public LinkDefinition[] Links { get; init; } = Array.Empty<LinkDefinition>();
    }
}
