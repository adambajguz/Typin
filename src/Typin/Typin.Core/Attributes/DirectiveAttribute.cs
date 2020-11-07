namespace Typin.Attributes
{
    using System;

    /// <summary>
    /// Annotates a type that defines a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DirectiveAttribute : Attribute
    {
        /// <summary>
        /// Directive name.
        /// All directives in an application must have different names.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Directive description, which is used in help text.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// List of CLI mode types, in which the directive can be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public Type[]? SupportedModes { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandAttribute"/>.
        /// </summary>
        public DirectiveAttribute(string name)
        {
            Name = name;
        }
    }
}