namespace TypinExamples.Infrastructure.WebWorkers.BlazorBoot
{
    using System;
    using System.Text.Json.Serialization;

    public class BlazorBootModel
    {
        public const string FilePath = "_framework/blazor.boot.json";

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
        public BlazorResourcesModel Resources { get; init; } = new BlazorResourcesModel();
    }
}
