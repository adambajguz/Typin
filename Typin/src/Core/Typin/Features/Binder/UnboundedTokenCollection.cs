namespace Typin.Features.Binder
{
    using System.Linq;
    using Typin.Features.Binding;
    using Typin.Features.Input;

    /// <summary>
    /// Default implementation of <see cref="IUnboundedTokenCollection"/>.
    /// </summary>
    public class UnboundedTokenCollection : TokenCollection, IUnboundedTokenCollection
    {
        /// <inheritdoc/>
        public bool HasUnbounded => Data.Count > 0 &&
                    Data.Any(x => x.Value.Tokens.Count > 0);

        /// <inheritdoc/>
        public bool IsBounded => !HasUnbounded;

        /// <summary>
        /// Initializes a new instance of <see cref="UnboundedTokenCollection"/>.
        /// </summary>
        public UnboundedTokenCollection(ITokenCollection inputCollection) :
            base()
        {
            Data = inputCollection.ToDictionary(
                x => x.Key,
                x => x.Value.DeepClone());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                ", " +
                $"{nameof(HasUnbounded)} = {HasUnbounded}";
        }
    }
}