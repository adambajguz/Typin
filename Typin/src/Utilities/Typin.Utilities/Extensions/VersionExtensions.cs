namespace Typin.Utilities.Extensions
{
    using System;

    /// <summary>
    /// <see cref="Version"/> extensions.
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// Gets a semantic string version, e.g. "1.2.3".
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string ToSemanticString(this Version version)
        {
            return version.Revision <= 0 ? version.ToString(3) : version.ToString();
        }
    }
}