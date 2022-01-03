namespace Typin.Exceptions.ArgumentBinding
{
    /// <summary>
    /// Unbounded tokens exception.
    /// </summary>
    public sealed class UnboundedTokensException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnboundedTokensException"/>.
        /// </summary>
        public UnboundedTokensException() :
            base(null,
                 "One or more tokens were not bounded.")
        {

        }
    }
}