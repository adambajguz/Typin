namespace TypinExamples.Validation.Middleware
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;
    using Typin;
    using Typin.Console;
    using Typin.Schemas;

    public sealed class FluentValidationMiddleware : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationMiddleware(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            try
            {
                Type validatorType = typeof(IValidator<>).MakeGenericType(context.CommandSchema.Type);

                if (_serviceProvider.GetService(validatorType) is IValidator validator)
                {
                    IValidationContext validationContext = new ValidationContext<ICommand>(context.Command);
                    ValidationResult validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }

                await next();
            }
            catch (ValidationException ex)
            {
                PrintValidationResults(context, ex);
            }
        }

        private static void PrintValidationResults(ICliContext context, ValidationException ex)
        {
            context.Console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine("Validation failed:"));

            foreach (IGrouping<string, ValidationFailure> group in ex.Errors.GroupBy(x => x.PropertyName))
            {
                ArgumentSchema property = context.CommandSchema.GetArguments().Where(x => x.Property?.Name == group.Key).First();

                string name = group.Key;
                if (property is CommandOptionSchema option)
                {
                    name = "--" + option.Name;
                }
                else if (property is CommandParameterSchema parameter)
                {
                    name = $"Parameter {parameter.Order}";
                }

                context.Console.Error.Write(" ");
                context.Console.Error.WithForegroundColor(ConsoleColor.Cyan, (error) => error.Write(name));

                context.Console.Error.Write(" ");
                context.Console.Error.WithForegroundColor(ConsoleColor.Green, (error) => error.Write($"[{group.First().AttemptedValue}]"));
                context.Console.Error.WriteLine(" ");

                foreach (var validationError in group)
                {
                    context.Console.Error.Write("   -- ");
                    context.Console.Error.WithForegroundColor(ConsoleColor.White, (error) => error.WriteLine(validationError.ErrorMessage));
                }

                context.Console.Error.WriteLine();
            }
        }
    }
}
