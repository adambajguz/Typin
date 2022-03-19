namespace Typin.Directives.Features
{
    using System;
    using Typin.Directives;

    /// <summary>
    /// Represents a directive instance that constists of a model and a handler.
    /// </summary>
    public sealed class DirectiveInstance
    {
        /// <summary>
        /// Directive identifier.
        /// </summary>
        public int DirectiveId { get; }

        /// <summary>
        /// Directive model.
        /// </summary>
        public IDirective Model { get; }

        /// <summary>
        /// Directive model handler.
        /// </summary>
        public IDirectiveHandler Handler { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveInstance"/>.
        /// </summary>
        /// <param name="directiveId"></param>
        /// <param name="model"></param>
        /// <param name="handler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveInstance(int directiveId, IDirective model, IDirectiveHandler handler)
        {
            DirectiveId = directiveId;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }
}
