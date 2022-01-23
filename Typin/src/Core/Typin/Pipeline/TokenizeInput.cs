namespace Typin.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    /// <summary>
    /// Tokenizes the input.
    /// </summary>
    public sealed class TokenizeInput : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TokenizeInput"/>.
        /// </summary>
        public TokenizeInput()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Tokenizer.Tokenize();

            await next();
        }
    }
}
