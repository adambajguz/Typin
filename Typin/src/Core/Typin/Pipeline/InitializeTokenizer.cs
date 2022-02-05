namespace Typin.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
    using Typin.Features.Tokenizer;
    using Typin.Features.Tokenizer.Handlers;

    /// <summary>
    /// Initializes tokenizer feature by rewriting Input.Arguments and Input.Options to TokenizerFeature to allow raw input interception and tokenization.
    /// </summary>
    public sealed class InitializeTokenizer : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InitializeTokenizer"/>.
        /// </summary>
        public InitializeTokenizer()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IInputFeature inputFeature = args.Input;
            List<ITokenHandler> handlers = new()
            {
                new NamedTokenHandler(),
                new ValueTokenHandler()
            }; //TODO: dynamic from services or options

            args.Features.Set<ITokenizerFeature>(new TokenizerFeature(inputFeature.Arguments, inputFeature.Options, handlers));

            await next();
        }
    }
}
