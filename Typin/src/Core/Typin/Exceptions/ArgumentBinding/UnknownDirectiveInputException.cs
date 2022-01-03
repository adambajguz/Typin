namespace Typin.Exceptions.ArgumentBinding
{
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Unknown directive input exception.
    /// </summary>
    public sealed class UnknownDirectiveInputException : ArgumentBindingException
    {
        /// <summary>
        /// Directive input.
        /// </summary>
        public DirectiveToken DirectiveInput { get; }

        /// <summary>
        /// Initializes an instance of <see cref="UnknownDirectiveInputException"/>.
        /// </summary>
        /// <param name="directive"></param>
        public UnknownDirectiveInputException(DirectiveToken directive) :
            base(null,
                 $"Unknown directive '{directive}'.")
        {
            DirectiveInput = directive;
        }
    }
}