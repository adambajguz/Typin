namespace Typin.Internal
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    internal class EnvironmentVariablesAccessor : IEnvironmentVariablesAccessor
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
    }
}