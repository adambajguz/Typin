namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// Scannable CLI components builder.
    /// </summary>
    public interface IScannableBuilder
    {
        /// <summary>
        /// Get or add a scanner.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="factory"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        TInterface GetScanner<TComponent, TInterface>(Func<ICliBuilder, IReadOnlyCollection<Type>?, TInterface> factory)
            where TComponent : class
            where TInterface : class, IScanner<TComponent>;
    }
}