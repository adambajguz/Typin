namespace Typin.Features.Input
{
    using System.Collections.Generic;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a collection of directives.
    /// </summary>
    public interface IDirectiveCollection : IList<IDirectiveToken>
    {
        /// <summary>
        /// Builds and returns a raw input.
        /// </summary>
        /// <returns></returns>
        IList<string> GetRaw();
    }
}
