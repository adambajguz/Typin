namespace Typin.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IOutputFeature"/> implementation.
    /// </summary>
    internal sealed class OutputFeature : IOutputFeature
    {
        /// <inheritdoc/>
        public int? ExitCode { get; set; }

        /// <inheritdoc/>
        public Dictionary<string, object?>? Data { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OutputFeature"/>.
        /// </summary>
        public OutputFeature()
        {

        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(ExitCode)} = {ExitCode?.ToString() ?? "null"}";
        }
    }
}
