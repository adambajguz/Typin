namespace TypinExamples.Validation.Commands
{
    using FluentValidation;

    public class SampleCommandValidator : AbstractValidator<SampleCommand>
    {
        public SampleCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage("Email is invalid.");

            RuleFor(x => x.Email).MinimumLength(3)
                                 .WithMessage("Email is too short.");

            RuleFor(x => x.Email2).EmailAddress()
                                 .WithMessage("Email2 is invalid.");

            RuleFor(x => x.Email2).MinimumLength(3)
                                 .WithMessage("Email2 is too short.");
        }
    }
}
