namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Input;
    using Typin.Schemas;

    internal sealed class BindInput : IMiddleware
    {
        private readonly IConfiguration _configuration;

        public BindInput(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            var context = args;

            //Get input and command schema from context
            CommandInput input = context.Input;
            CommandSchema commandSchema = context.CommandSchema;
            //Type currentModeType = _applicationLifetime.CurrentModeType!;

            //// Handle commands not supported in current mode
            //if (!commandSchema.CanBeExecutedInMode(currentModeType))
            //{
            //    throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);
            //}

            // Get command instance from context and bind arguments
            ICommand instance = context.Command;
            commandSchema.BindParameters(instance, input.Parameters);
            commandSchema.BindOptions(instance, input.Options, _configuration);

            await next();
        }
    }
}
