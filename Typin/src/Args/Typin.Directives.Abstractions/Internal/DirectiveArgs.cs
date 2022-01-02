namespace Typin.Directives.Internal
{
    using System;
    using Typin;
    using Typin.Directives;

    /// <summary>
    /// Directive arguments.
    /// </summary>
    internal sealed class DirectiveArgs<TDirective> : IDirectiveArgs<TDirective>
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Directive data.
        /// </summary>
        public TDirective Directive { get; }
        IDirective IDirectiveArgs.Directive => Directive;

        /// <summary>
        /// Current CLI context.
        /// </summary>
        public CliContext Context { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="context"></param>
        public DirectiveArgs(TDirective directive, CliContext context)
        {
            Directive = directive ?? throw new ArgumentNullException(nameof(directive));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveArgs{TDirective}"/>.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="context"></param>
        public DirectiveArgs(IDirective directive, CliContext context)
        {
            Directive = directive as TDirective ?? throw new ArgumentNullException(nameof(directive));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
