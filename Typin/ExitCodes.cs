namespace Typin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Exceptions;

    /// <summary>
    /// Static exit codes helper class.
    /// </summary>
    public static class ExitCodes
    {
        /// <summary>
        /// Success exit code.
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// Error exit code.
        /// </summary>
        public const int Error = 1;

        /// <summary>
        /// Gets an exit code from exception.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static int FromException(Exception ex)
        {
            return ex is CommandException cmdEx ? cmdEx.ExitCode : Error;
        }
    }
}
