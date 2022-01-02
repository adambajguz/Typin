namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Collections.Generic;

    public sealed class WorkerCreationConfiguration
    {
        /// <summary>
        /// Collection of assemblies to load, e.g. new string[] { "System.Text.Json.dll" }. When empty (default) all assemblies are loaded.
        /// IncludedAssemlies collection has lower priority than ExcludedAssemlbies, i.e., when assemlby is in both collections it will be excluded.
        /// </summary>
        public ICollection<string> IncludedAssemblies { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Collection of assemblies to exclude from loading, e.g. new string[] { "System.Text.Json.dll" }. When empty (default) all assemlbies are loaded.
        /// </summary>
        public ICollection<string> ExcludedAssemblied { get; set; } = Array.Empty<string>();
    }
}
