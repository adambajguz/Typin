namespace TypinExamples.Common.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class BlazorBoot
    {
        [JsonPropertyName("cacheBootResources")]
        public bool CacheBootResources { get; init; }

        [JsonPropertyName("config")]
        public string[] Config { get; init; } = Array.Empty<string>();

        [JsonPropertyName("debugBuild")]
        public bool DebugBuild { get; init; }

        [JsonPropertyName("entryAssembly")]
        public string EntryAssembly { get; init; } = string.Empty;

        [JsonPropertyName("linkerEnabled")]
        public bool LinkerEnabled { get; init; }

        [JsonPropertyName("resources")]
        public Resources Resources { get; init; } = new Resources();
    }
}
