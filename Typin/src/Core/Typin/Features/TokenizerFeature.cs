namespace Typin.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Typin;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Features.Tokenizer;

    /// <summary>
    /// <see cref="ITokenizerFeature"/> implementation.
    /// </summary>
    internal sealed class TokenizerFeature : ITokenizerFeature
    {
        private int _idSource;

        /// <inheritdoc/>
        public IEnumerable<string> Input { get; set; }

        /// <inheritdoc/>
        public InputOptions InputOptions { get; set; }

        /// <inheritdoc/>
        public IList<ITokenHandler> Handlers { get; set; }

        /// <inheritdoc/>
        public IDirectiveCollection? Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFeature"/>.
        /// </summary>
        public TokenizerFeature(IEnumerable<string> input,
                                InputOptions inputOptions)
        {
            Input = input;
            InputOptions = inputOptions;
            Handlers = new List<ITokenHandler>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFeature"/>.
        /// </summary>
        public TokenizerFeature(IEnumerable<string> input,
                                InputOptions inputOptions,
                                IList<ITokenHandler> handlers) :
            this(input, inputOptions)
        {
            Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        /// <inheritdoc/>
        public void Tokenize()
        {
            Tokens = Tokenize(Input, InputOptions);
        }

        /// <inheritdoc/>
        public IDirectiveCollection Tokenize(IEnumerable<string> arguments, InputOptions options)
        {
            bool trimExecutable = options.HasFlag(InputOptions.TrimExecutable);
            List<string> tokenizerInput = arguments.Skip(trimExecutable ? 1 : 0).ToList();

            DirectiveCollection directives = new();

            if (tokenizerInput.Count > 0)
            {
                ParseDirectives(directives, tokenizerInput);
            }

            return directives;
        }

        private void ParseDirectives(IDirectiveCollection directives,
                                     IReadOnlyList<string> args)
        {
            for (int index = 0; index < args.Count; index++)
            {
                string argument = args[index].Trim();

                DirectiveToken directiveToken = new(Interlocked.Increment(ref _idSource), argument);
                directives.Add(directiveToken);

                if (!directiveToken.IsTerminated)
                {
                    TokenHandlerContext tokenHandlerContext = new(directiveToken,
                                                                  args,
                                                                  index);

                    for (int i = 0; i < Handlers.Count; i++)
                    {
                        ITokenHandler handler = Handlers[i];

                        if (handler.CanHandle(tokenHandlerContext))
                        {
                            if (handler.Handle(tokenHandlerContext))
                            {
                                break;
                            }
                        }
                    }

                    index = tokenHandlerContext.Position;
                }
            }
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Input)} = [{string.Join(';', Input)}], " +
                $"{nameof(InputOptions)} = {InputOptions}, " +
                $"{nameof(Handlers)} = [{string.Join(';', Handlers)}], " +
                $"{nameof(Tokens)} = {{{Tokens}}}";
        }
    }
}
