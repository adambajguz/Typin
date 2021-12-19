namespace Typin.Internal
{
    using System;
    using System.IO;
    using System.Reflection;

    internal static class AssemblyUtils
    {
        private static readonly Lazy<Assembly?> LazyEntryAssembly = new(Assembly.GetEntryAssembly);

        /// <summary>
        /// Entry assembly is null in tests.
        /// </summary>
        public static Assembly? EntryAssembly => LazyEntryAssembly.Value;

        public static string? TryGetDefaultExecutableName()
        {
            string? entryAssemblyLocation = EntryAssembly?.Location;

            // The assembly can be an executable or a dll, depending on how it was packaged
            bool isDll = string.Equals(Path.GetExtension(entryAssemblyLocation), ".dll", StringComparison.OrdinalIgnoreCase);

            return isDll
                ? $"dotnet {Path.GetFileName(entryAssemblyLocation)}"
                : Path.GetFileNameWithoutExtension(entryAssemblyLocation);
        }

        public static string GetDefaultVersionText()
        {
            return EntryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ??
                EntryAssembly?.GetName()?.Version?.ToString() ??
                "1.0.0";
        }
    }
}
