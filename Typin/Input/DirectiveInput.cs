namespace Typin.Input
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Stores command directive input.
    /// </summary>
    public class DirectiveInput
    {
        /// <summary>
        /// Directive name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveInput"/>.
        /// </summary>
        public DirectiveInput(string name)
        {
            Name = name;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return string.Concat("[", Name, "]");
        }
    }
}