namespace Typin.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    internal static class ProcessEx
    {
        [ExcludeFromCodeCoverage]
        public static int GetCurrentProcessId()
        {
            using Process process = Process.GetCurrentProcess();

            return process.Id;
        }
    }
}