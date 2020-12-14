namespace TypinExamples.Configurations
{
    using System;

    public sealed class HeaderConfiguration
    {
        /// <summary>
        /// Whether header is full screen height.
        /// </summary>
        public bool IsFullScreen { get; init; }

        /// <summary>
        /// Heading with Markdown formatting.
        /// </summary>
        public string? Heading { get; init; }

        /// <summary>
        /// Subheading lines with markdown formatting.
        /// </summary>
        public string[]? Subheading { get; init; }

        /// <summary>
        /// Subheading with markdown formatting.
        /// </summary>
        public string SubheadingText => Subheading is null ? string.Empty : string.Join("\n", Subheading);

        /// <summary>
        /// Links colleciton.
        /// </summary>
        public LinkDefinition[] Links { get; init; } = Array.Empty<LinkDefinition>();
    }
}
