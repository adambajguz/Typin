namespace Typin.Directives
{
    using System;
    using Typin;
    using Typin.Directives.Features;

    /// <summary>
    /// Represents a directive instance that consists of a model and a handler.
    /// </summary>
    public class DirectiveArgs<TDirective> : DirectiveArgs
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Directive model.
        /// </summary>
        public new TDirective Model { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <param name="directiveId"></param>
        /// <param name="model"></param>
        /// <param name="handler"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveArgs(int directiveId, TDirective model, IDirectiveHandler handler, CliContext context) :
            base(directiveId, model, handler, context)
        {
            Model = model;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveArgs(DirectiveInstance instance, CliContext context) :
            this(instance.DirectiveId, (TDirective)instance.Model, instance.Handler, context)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectiveArgs(DirectiveArgs args) :
            this(args.DirectiveId, (TDirective)args.Model, args.Handler, args.Context)
        {

        }

        /// <summary>
        /// Converts <see cref="DirectiveArgs{TDirective}"/> to <typeparamref name="TDirective"/>.
        /// </summary>
        /// <param name="args"></param>
        public static implicit operator TDirective(DirectiveArgs<TDirective> args)
        {
            return args.Model;
        }
    }
}
