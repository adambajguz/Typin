namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
    using Typin.Input;
    using Typin.Internal.Input;
    using Typin.Schemas;

    /// <summary>
    /// Binds input.
    /// </summary>
    public sealed class BindInput : IMiddleware
    {
        private readonly IConfiguration _configuration; //TODO: wrap IConfiguration with Typin interface to allow decoupling from IConfiguration

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
            ParsedCommandInput input = args.Input.Parsed ??
                throw new InvalidOperationException($"{nameof(IInputFeature)}.{nameof(IInputFeature.Parsed)} has not been configured for this application or call.");

            CommandSchema commandSchema = args.Command.Schema;

            //Type currentModeType = _applicationLifetime.CurrentModeType!;

            //// Handle commands not supported in current mode
            //if (!commandSchema.CanBeExecutedInMode(currentModeType))
            //{
            //    throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);
            //}

            // Get command instance from context and bind arguments
            ICommand instance = args.Command.Instance;

            commandSchema.BindParameters(instance, input);
            commandSchema.BindOptions(instance, input, _configuration);

            await next();
        }
    }
}
