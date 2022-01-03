namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Command line input feature.
    /// </summary>
    public interface IInputFeature
    {
        /// <summary>
        /// Original raw command line input arguments.
        /// </summary>
        IEnumerable<string> Original { get; }

        /// <summary>
        /// Command execution options.
        /// </summary>
        InputOptions Options { get; }

        /// <summary>
        /// Tokenized CLI input as a set of directives.
        /// </summary>
        IDirectiveCollection? Tokens { get; set; }
    }
}
