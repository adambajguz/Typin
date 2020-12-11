namespace TypinExamples.Validation.Commands
{
    using FluentValidation;

    public class OtherCommandValidator : AbstractValidator<OtherCommand>
    {
        public OtherCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .MinimumLength(2)
                                .WithMessage("Name is invalid");
        }
    }
}
