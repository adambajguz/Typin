namespace Typin.Features
{
    using System.Diagnostics;

    /// <summary>
    /// <see cref="IBinderFeature"/> implementation.
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal sealed class BinderFeature : IBinderFeature
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BinderFeature"/>.
        /// </summary>
        public BinderFeature()
        {

        }

        private string GetDebuggerDisplay()
        {
            return ToString() +
                " | ";
        }
    }
}
