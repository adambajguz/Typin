namespace Typin.Exceptions.ArgumentBinding
{
    using Typin.Input;

    /// <summary>
    /// Unknown directive input exception.
    /// </summary>
    public sealed class UnknownDirectiveInputException : ArgumentBindingException
    {
        /// <summary>
        /// Directive input.
        /// </summary>
        public DirectiveInput DirectiveInput { get; }

        /// <summary>
        /// Initializes an instance of <see cref="UnknownDirectiveInputException"/>.
        /// </summary>
        /// <param name="directive"></param>
        public UnknownDirectiveInputException(DirectiveInput directive) :
            base(null,
                 $"Unknown directive '{directive}'.")
        {
            DirectiveInput = directive;
        }
    }
}