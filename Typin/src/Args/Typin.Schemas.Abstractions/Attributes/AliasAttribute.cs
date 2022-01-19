namespace Typin.Schemas.Attributes
{
    using System;

    /// <summary>
    /// Annotates with an alias.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class AliasAttribute : Attribute
    {
        /// <summary>
        /// Alias value.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="AliasAttribute"/>.
        /// </summary>
        public AliasAttribute()
        {
            Value = string.Empty;
        }

        /// <summary>
        /// Initializes an instance of <see cref="AliasAttribute"/>.
        /// </summary>
        public AliasAttribute(string alias)
        {
            Value = alias ?? throw new ArgumentNullException(nameof(alias));
        }
    }
}