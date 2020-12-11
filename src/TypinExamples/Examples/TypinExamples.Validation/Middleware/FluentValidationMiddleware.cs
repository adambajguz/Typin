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
        public FluentValidationMiddleware()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            //var context = new ValidationContext(request);

            //var failures = _validators
            //    .Select(v => v.Validate(context))
            //    .SelectMany(result => result.Errors)
            //    .Where(f => f != null)
            //    .ToList();

            //if (failures.Count != 0)
            //{
            //    throw new ValidationException(failures);
            //}

            try
            {
                await next();
            }
            catch (ValidationException ex)
            {
                context.Console.WithForegroundColor(ConsoleColor.Red, () => context.Console.Output.WriteLine("Validation failed:"));

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

                    context.Console.Output.Write(" ");
                    context.Console.WithForegroundColor(ConsoleColor.Cyan, () => context.Console.Output.Write(name));

                    context.Console.Output.Write(" ");
                    context.Console.WithForegroundColor(ConsoleColor.Green, () => context.Console.Output.Write($"[{group.First().AttemptedValue}]"));
                    context.Console.Output.WriteLine(" ");

                    foreach (var error in group)
                    {
                        context.Console.Output.Write("   -- ");
                        context.Console.WithForegroundColor(ConsoleColor.White, () => context.Console.Output.WriteLine(error.ErrorMessage));
                    }

                    context.Console.Output.WriteLine();
                }
            }
        }
    }
}
