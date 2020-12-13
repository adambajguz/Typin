namespace Typin
{
    using System.Collections.Generic;

    /// <summary>
    /// Service that can be used to access environment variables passed to CliApplication.RunAsync.
    /// </summary>
    public interface IEnvironmentVariablesAccessor
    {
        /// <summary>
        /// Environment variables.
        /// </summary>
        IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
    }
}