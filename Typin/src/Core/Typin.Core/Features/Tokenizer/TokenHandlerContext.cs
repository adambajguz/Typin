namespace Typin.Features.Tokenizer
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a <see cref="ITokenHandler{TToken}"/> context.
    /// </summary>
    public sealed class TokenHandlerContext
    {
        /// <summary>
        /// Tokens collection
        /// </summary>
        public IDirectiveToken Directive { get; }

        /// <summary>
        /// Tokens collection - an alias to <see cref="Directive"/>.Children.
        /// </summary>
        public ITokenCollection Tokens { get; }

        /// <summary>
        /// Arguments being tokenized.
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// Position/Index in <see cref="Tokens"/> to start <see cref="ITokenHandler.Handle(TokenHandlerContext)"/> logic from.
        /// This must be manually updated inside <see cref="ITokenHandler.Handle(TokenHandlerContext)"/> before exiting.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TokenHandlerContext"/>.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="arguments"></param>
        /// <param name="position"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TokenHandlerContext(IDirectiveToken directive,
                                   IReadOnlyList<string> arguments,
                                   int position)
        {
            Directive = directive ?? throw new ArgumentNullException(nameof(directive));
            Tokens = directive.Children;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Position = position;
        }
    }
}
