//namespace Typin.Exceptions.Resolvers.DirectiveResolver
//{
//    using System;
//    using Typin.Models.Attributes; using Typin.Commands.Attributes;

//    /// <summary>
//    /// Invalid pipelined directive exception.
//    /// </summary>
//    public sealed class InvalidPipelinedDirectiveException : DirectiveResolverException
//    {
//        /// <summary>
//        /// Directive type.
//        /// </summary>
//        public Type Type { get; set; }

//        /// <summary>
//        /// Initializes an instance of <see cref="InvalidDirectiveException"/>.
//        /// </summary>
//        /// <param name="type"></param>
//        public InvalidPipelinedDirectiveException(Type type) :
//            base(BuildMessage(type))
//        {
//            Type = type;
//        }

//        private static string BuildMessage(Type type)
//        {
//            return $"Directive '{type.FullName}' is not a valid pipelined directive type.{Environment.NewLine}" +
//                   Environment.NewLine +
//                   $"In order to be a valid pipelined directive type, it must:{Environment.NewLine}" +
//                   $"  - Not be an abstract class{Environment.NewLine}" +
//                   $"  - Implement {typeof(IPipelinedDirective).FullName}{Environment.NewLine}" +
//                   $"  - Be annotated with {typeof(DirectiveAttribute).FullName}.";
//        }
//    }
//}