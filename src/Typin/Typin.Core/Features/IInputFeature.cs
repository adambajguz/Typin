namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Input;

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
        CommandExecutionOptions ExecutionOptions { get; }

        /// <summary>
        /// Parsed CLI input.
        /// </summary>
        ParsedCommandInput? Parsed { get; set; }
    }
}
