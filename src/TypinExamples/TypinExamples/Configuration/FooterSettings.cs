namespace TypinExamples.Configuration
{
    using System;

    public sealed class FooterSettings
    {
        /// <summary>
        /// Authors with Markdown formatting.
        /// </summary>
        public string? Authors { get; set; }

        /// <summary>
        /// Links colleciton.
        /// </summary>
        public LinkDefinition[] Links { get; set; } = Array.Empty<LinkDefinition>();
    }
}
