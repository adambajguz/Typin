namespace Typin.Internal.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    internal static class StringBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static StringBuilder AppendIfNotEmpty(this StringBuilder builder, char value)
        {
            return builder.Length > 0 ? builder.Append(value) : builder;
        }
    }
}