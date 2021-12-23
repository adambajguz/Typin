namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
    using Typin.Features.Binding;
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
            ParsedInput input = args.Input.Parsed ??
                throw new InvalidOperationException($"{nameof(IInputFeature)}.{nameof(IInputFeature.Parsed)} has not been configured for this application or call.");

            UnboundedInput unboundedInput = new(input);
            args.Features.Set<IBinderFeature>(new BinderFeature(unboundedInput));

            await next();
        }
    }
}
