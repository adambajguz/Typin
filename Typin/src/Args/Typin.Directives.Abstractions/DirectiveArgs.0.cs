namespace Typin.Directives
{
    using System;
    using Typin.Directives.Features;

    /// <summary>
    /// Represents a directive instance that consists of a model and a handler.
    /// </summary>
    public class DirectiveArgs
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
        /// CLI context.
        /// </summary>
        public CliContext Context { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs"/>.
        /// </summary>
        /// <param name="directiveId"></param>
        /// <param name="model"></param>
        /// <param name="handler"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveArgs(int directiveId, IDirective model, IDirectiveHandler handler, CliContext context)
        {
            DirectiveId = directiveId;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveArgs(DirectiveInstance instance, CliContext context)
        {
            DirectiveId = instance.DirectiveId;
            Model = instance.Model;
            Handler = instance.Handler;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Converts this instance to <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <returns></returns>
        public DirectiveArgs<TDirective> As<TDirective>()
            where TDirective : class, IDirective
        {
            return new DirectiveArgs<TDirective>(this);
        }

        /// <summary>
        /// Converts <see cref="DirectiveArgs"/> to <see cref="CliContext"/>.
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator CliContext(DirectiveArgs args)
        {
            return args.Context;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(DirectiveId)} = {DirectiveId}, " +
                $"{nameof(Model)} = {Model}, " +
                $"{nameof(Handler)} = {Handler}, " +
                $"{nameof(Context)} = {{{Context}}}";
        }
    }
}
