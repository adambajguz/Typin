namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Input;
    using Typin.Schemas;

    /// <summary>
    /// Binds input.
    /// </summary>
    public sealed class BindInput : IMiddleware
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of <see cref="BindInput"/>.
        /// </summary>
        public BindInput(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            //Get input and command schema from context
            ParsedCommandInput input = args.Input ?? throw new NullReferenceException($"{nameof(CliContext.Input)} must be set in {nameof(CliContext)}.");
            CommandSchema commandSchema = args.CommandSchema ?? throw new NullReferenceException($"{nameof(CliContext.CommandSchema)} must be set in {nameof(CliContext)}.");
            //Type currentModeType = _applicationLifetime.CurrentModeType!;

            //// Handle commands not supported in current mode
            //if (!commandSchema.CanBeExecutedInMode(currentModeType))
            //{
            //    throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);
            //}

            // Get command instance from context and bind arguments
            ICommand instance = args.Command ?? throw new NullReferenceException($"{nameof(CliContext.Command)} must be set in {nameof(CliContext)}.");
            commandSchema.BindParameters(instance, input);
            commandSchema.BindOptions(instance, input, _configuration);

            await next();
        }
    }
}
