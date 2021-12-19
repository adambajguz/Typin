namespace Typin.Features
{
    using System.Diagnostics;
    using Typin.Input;

    /// <summary>
    /// <see cref="IBinderFeature"/> implementation.
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal sealed class BinderFeature : IBinderFeature
    {
        /// <inheritdoc/>
        public UnboundedInput UnboundedInput { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BinderFeature"/>.
        /// </summary>
        public BinderFeature(UnboundedInput unboundedInput)
        {
            UnboundedInput = unboundedInput;
        }

        private string GetDebuggerDisplay()
        {
            return ToString() +
                " | ";
        }
    }
}
