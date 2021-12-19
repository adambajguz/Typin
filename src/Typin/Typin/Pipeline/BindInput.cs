namespace Typin.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
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
            IBinderFeature binder = args.Binder;

            CommandSchema commandSchema = args.Command.Schema;

            //Type currentModeType = _applicationLifetime.CurrentModeType!;

            //// Handle commands not supported in current mode
            //if (!commandSchema.CanBeExecutedInMode(currentModeType))
            //{
            //    throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);
            //}

            // Get command instance from context and bind arguments
            ICommand instance = args.Command.Instance;

            commandSchema.BindParameters(instance, binder.UnboundedInput);
            commandSchema.BindOptions(instance, binder.UnboundedInput, _configuration);

            //await Task.Delay(500, cancellationToken); //TODO: remove

            //args.Lifetime.Abort(); //TODO: remove

            //await Task.Delay(500, cancellationToken); //TODO: remove

            await next();
        }
    }
}
