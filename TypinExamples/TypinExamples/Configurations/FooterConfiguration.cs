﻿namespace TypinExamples.Configurations
{
    using System;

    public sealed class FooterConfiguration
    {
        /// <summary>
        /// Authors with Markdown formatting.
        /// </summary>
        public string? Authors { get; init; }

        /// <summary>
        /// Links colleciton.
        /// </summary>
        public LinkDefinition[] Links { get; init; } = Array.Empty<LinkDefinition>();
    }
}
