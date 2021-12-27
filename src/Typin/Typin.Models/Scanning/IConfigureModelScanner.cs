namespace Typin.Models.Scanning
{
    using System;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureModel"/> component scanner.
    /// </summary>
    public interface IConfigureModelScanner : IScanner<IConfigureModel>
    {
        /// <summary>
        /// Adds <see cref="IConfigureModel"/> from nested classes in models.
        /// </summary>
        IConfigureModelScanner FromNested(Type type);
    }
}