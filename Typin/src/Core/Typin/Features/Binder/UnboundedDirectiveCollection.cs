namespace Typin.Features.Binder
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Binding;
    using Typin.Features.Input;

    /// <summary>
    /// Default implementation of <see cref="IUnboundedDirectiveCollection"/>.
    /// </summary>
    public class UnboundedDirectiveCollection : List<IUnboundedDirectiveToken>, IUnboundedDirectiveCollection
    {
        /// <inheritdoc/>
        public bool HasUnbounded => Count > 0 &&
                    this.Any(x => x.HasUnbounded);

        /// <inheritdoc/>
        public bool IsBounded => !HasUnbounded;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenCollection"/>.
        /// </summary>
        public UnboundedDirectiveCollection(IDirectiveCollection directives) :
            base(directives.Select(x => new UnboundedDirectiveToken(x)))
        {

        }
    }
}