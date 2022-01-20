namespace Typin.Features.Binder
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Default implementation of <see cref="IUnboundedDirectiveCollection"/>.
    /// </summary>
    public class UnboundedDirectiveCollection : IUnboundedDirectiveCollection
    {
        /// <summary>
        /// Data.
        /// </summary>
        protected Dictionary<int, IUnboundedDirectiveToken> Data { get; }

        /// <inheritdoc/>
        public bool HasUnbounded => Data.Count > 0 && this.Any(x => x.HasUnbounded);

        /// <inheritdoc/>
        public bool IsBounded => !HasUnbounded;

        /// <inheritdoc/>
        public int Count => Data.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public IUnboundedDirectiveToken? this[int id] => Data.GetValueOrDefault(id);

        /// <summary>
        /// Initializes a new instance of <see cref="TokenCollection"/>.
        /// </summary>
        public UnboundedDirectiveCollection(IDirectiveCollection directives)
        {
            Data = directives.ToDictionary<IDirectiveToken, int, IUnboundedDirectiveToken>(
                x => x.Id,
                x => new UnboundedDirectiveToken(x));
        }

        /// <inheritdoc />
        public void Add(IUnboundedDirectiveToken item)
        {
            Data.Add(item.Id, item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Data.Clear();
        }

        /// <inheritdoc />
        public bool Contains(IUnboundedDirectiveToken item)
        {
            return Data.ContainsKey(item.Id) && Data.ContainsValue(item);
        }

        /// <inheritdoc />
        public void CopyTo(IUnboundedDirectiveToken[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                IUnboundedDirectiveToken item = array[i];

                Data.Add(item.Id, item);
            }
        }

        /// <inheritdoc />
        public bool Remove(IUnboundedDirectiveToken item)
        {
            return Data.Remove(item.Id);
        }

        /// <inheritdoc />
        public bool Remove(int id)
        {
            return Data.Remove(id);
        }

        /// <inheritdoc />
        public IEnumerator<IUnboundedDirectiveToken> GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(HasUnbounded)} = {HasUnbounded}, " +
                $"{nameof(Count)} = {Count}";
        }
    }
}