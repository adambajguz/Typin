namespace Typin.Exceptions
{
    using System;

    /// <summary>
    /// Domain exception thrown within Typin.
    /// </summary>
    public sealed class TypinException : Exception
    {
        private readonly bool _isMessageSet;

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/> with a specified error message.
        /// </summary>
        public TypinException(string? message)
            : base(message)
        {
            _isMessageSet = !string.IsNullOrWhiteSpace(message);
        }

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/> with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public TypinException(string? message, Exception? innerException)
            : base(message, innerException!)
        {
            _isMessageSet = !string.IsNullOrWhiteSpace(message);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _isMessageSet ? Message : base.ToString();
        }
    }
}