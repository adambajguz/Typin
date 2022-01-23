namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
    using Typin.Features.Input;

    /// <summary>
    /// Initializes binder.
    /// </summary>
    public sealed class InitializeBinder : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InitializeBinder"/>.
        /// </summary>
        public InitializeBinder()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IDirectiveCollection tokens = args.Tokenizer.Tokens ??
                throw new InvalidOperationException($"{nameof(ITokenizerFeature)}.{nameof(ITokenizerFeature.Tokens)} has not been configured for this application or call.");

            args.Features.Set<IBinderFeature>(new BinderFeature(tokens));

            await next();

            args.Binder.Validate();
        }
    }
}
