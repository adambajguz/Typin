namespace Typin.Internal.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// <see cref="StringBuilder"/> extensions.
    /// </summary>
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Apppends <see cref="char"/> is builder is not empty.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static StringBuilder AppendIfNotEmpty(this StringBuilder builder, char value)
        {
            return builder.Length > 0
                ? builder.Append(value)
                : builder;
        }
    }
}