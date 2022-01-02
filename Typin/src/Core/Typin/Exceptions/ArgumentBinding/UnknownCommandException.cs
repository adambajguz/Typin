namespace Typin.Exceptions.ArgumentBinding
{
    /// <summary>
    /// Unknown command exception.
    /// </summary>
    public sealed class UnknownCommandException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnknownDirectiveInputException"/>.
        /// </summary>
        /// <param name="commandName"></param>
        public UnknownCommandException(string commandName) :
            base(null,
                 $"Unknown command '{commandName}'.")
        {

        }
    }
}