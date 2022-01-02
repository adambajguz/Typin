﻿namespace TypinExamples.Infrastructure.WebWorkers.BlazorBoot
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BlazorResourcesModel
    {
        [JsonPropertyName("assembly")]
        public Dictionary<string, string> Assembly { get; init; } = new Dictionary<string, string>();

        [JsonPropertyName("pdb")]
        public Dictionary<string, string> Pdb { get; init; } = new Dictionary<string, string>();

        [JsonPropertyName("runtime")]
        public Dictionary<string, string> Runtime { get; init; } = new Dictionary<string, string>();
    }
}
