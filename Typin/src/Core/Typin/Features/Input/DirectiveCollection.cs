namespace Typin.Features.Input
{
    using System.Collections.Generic;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Default implementation of <see cref="IDirectiveCollection"/>.
    /// </summary>
    public class DirectiveCollection : List<IDirectiveToken>, IDirectiveCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TokenCollection"/>.
        /// </summary>
        public DirectiveCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="TokenCollection"/>.
        /// </summary>
        public DirectiveCollection(IEnumerable<IDirectiveToken> values) :
            base(values)
        {

        }

        /// <inheritdoc/>
        public IList<string> GetRaw()
        {
            List<string> tmp = new();

            foreach (IDirectiveToken directive in this)
            {
                tmp.AddRange(directive.Raw);
            }

            return tmp;
        }
    }
}