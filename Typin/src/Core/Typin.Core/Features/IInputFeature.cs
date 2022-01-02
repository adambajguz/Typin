namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Features.Input;

    /// <summary>
    /// Command line input feature.
    /// </summary>
    public interface IInputFeature
    {
        /// <summary>
        /// Raw command line input arguments.
        /// </summary>
        IEnumerable<string> Arguments { get; }

        /// <summary>
        /// Command execution options.
        /// </summary>
        InputOptions Options { get; }

        /// <summary>
        /// Parsed CLI input.
        /// </summary>
        ParsedInput? Parsed { get; set; }
    }
}
