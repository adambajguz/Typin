namespace Typin.Exceptions.Resolvers.DirectiveResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Directive duplicate by name exception.
    /// </summary>
    public sealed class DirectiveDuplicateByNameException : DirectiveResolverException
    {
        /// <summary>
        /// Duplicated directive name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Duplicated directive name.
        /// </summary>
        public IReadOnlyList<DirectiveSchema> InvalidDirectives { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveDuplicateByNameException"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="invalidDirectives"></param>
        public DirectiveDuplicateByNameException(string name, IReadOnlyList<DirectiveSchema> invalidDirectives) :
            base(BuildMessage(name, invalidDirectives))
        {
            Name = name;
            InvalidDirectives = invalidDirectives;
        }

        private static string BuildMessage(string name, IReadOnlyList<DirectiveSchema> invalidDirectives)
        {
            return $"Application configuration is invalid because it contains {invalidDirectives.Count} directives with the same name ('{name}'):{Environment.NewLine}" +
                   $"{invalidDirectives.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Directives must have unique names (names are case-sensitive).";
        }
    }
}