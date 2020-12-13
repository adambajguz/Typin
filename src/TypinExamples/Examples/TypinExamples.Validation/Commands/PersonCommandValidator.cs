namespace TypinExamples.Validation.Commands
{
    using System.Linq;
    using FluentValidation;

    public class PersonCommandValidator : AbstractValidator<PersonCommand>
    {
        public PersonCommandValidator()
        {
            RuleFor(x => x.Name).MinimumLength(2)
                                .WithMessage("Name must be at least 2 charasters long.");

            RuleFor(x => x.Name).Custom((x, context) =>
            {
                if (x is null || !x.All(y => char.IsLetter(y)))
                {
                    context.AddFailure($"{x} is not a valid name.");
                }
            });

            RuleFor(x => x.Age).GreaterThanOrEqualTo(1)
                               .LessThan(150)
                               .WithMessage("Must be withing range <1, 150).");
        }
    }
}
