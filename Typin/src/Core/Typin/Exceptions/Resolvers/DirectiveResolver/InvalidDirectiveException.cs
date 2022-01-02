namespace Typin.Exceptions.Resolvers.DirectiveResolver
{
    using System;
    using Typin.Directives;
    using Typin.Directives.Attributes;

    /// <summary>
    /// Invalid directive exception.
    /// </summary>
    public sealed class InvalidDirectiveException : DirectiveResolverException
    {
        /// <summary>
        /// Directive type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InvalidDirectiveException"/>.
        /// </summary>
        /// <param name="type"></param>
        public InvalidDirectiveException(Type type) :
            base(BuildMessage(type))
        {
            Type = type;
        }

        private static string BuildMessage(Type type)
        {
            return $"Directive '{type.FullName}' is not a valid directive type.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"In order to be a valid directive type, it must:{Environment.NewLine}" +
                   $"  - Not be an abstract class{Environment.NewLine}" +
                   $"  - Implement {typeof(IDirective).FullName}{Environment.NewLine}" +
                   $"  - Be annotated with {typeof(DirectiveAttribute).FullName}.";
        }
    }
}