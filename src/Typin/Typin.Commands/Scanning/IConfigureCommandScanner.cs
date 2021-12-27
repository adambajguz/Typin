namespace Typin.Commands.Scanning
{
    using System;
    using Typin.Commands;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureCommand"/> component scanner.
    /// </summary>
    public interface IConfigureCommandScanner : IScanner<IConfigureCommand>
    {
        /// <summary>
        /// Adds <see cref="IConfigureCommand"/> from nested classes.
        /// </summary>
        IConfigureCommandScanner FromNested(Type type);
    }
}