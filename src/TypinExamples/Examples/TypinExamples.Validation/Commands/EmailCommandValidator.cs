namespace TypinExamples.Validation.Commands
{
    using FluentValidation;

    public class EmailCommandValidator : AbstractValidator<EmailCommand>
    {
        public EmailCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage("Email is empty.");

            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage("Email is invalid.");

            RuleFor(x => x.Email).MinimumLength(3)
                                 .WithMessage("Email is too short.");
        }
    }
}
