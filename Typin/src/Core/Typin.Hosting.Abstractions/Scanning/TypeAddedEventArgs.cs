namespace Typin.Hosting.Scanning
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a type was added.
    /// </summary>
    public sealed class TypeAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeAddedEventArgs"/>.
        /// </summary>
        /// <param name="pipelineName"></param>
        public TypeAddedEventArgs(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}