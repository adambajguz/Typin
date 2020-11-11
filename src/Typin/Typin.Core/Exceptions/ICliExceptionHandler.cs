namespace Typin.Exceptions
{
    using System;

    /// <summary>
    /// Abstraction for exception handling.
    /// </summary>
    public interface ICliExceptionHandler
    {
        /// <summary>
        /// Handles exception and returns true when exception was handled or false when not.
        /// </summary>
        bool HandleException(Exception ex);
    }
}
