namespace Typin.Exceptions
{
    using System;

    /// <summary>
    /// Abstraction for exception handling.
    /// </summary>
    public interface ICliExceptionHandler
    {
        /// <summary>
        /// Handles exception of <see cref="TypinException"/>.
        /// </summary>
        void HandleTypinException(ICliContext context, TypinException ex);

        /// <summary>
        /// Handles exception of <see cref="DirectiveException"/>.
        /// </summary>
        void HandleDirectiveException(ICliContext context, DirectiveException ex);

        /// <summary>
        /// Handles exception of <see cref="CommandException"/>.
        /// </summary>
        void HandleCommandException(ICliContext context, CommandException ex);

        /// <summary>
        /// Handles exception of <see cref="Exception"/>.
        /// </summary>
        void HandleException(ICliContext context, Exception ex);
    }
}
