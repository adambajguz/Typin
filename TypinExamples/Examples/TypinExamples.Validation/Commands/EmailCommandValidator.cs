namespace TypinExamples.Validation.Commands
{
    using FluentValidation;

    public class EmailCommandValidator : AbstractValidator<EmailCommand>
    {
        public EmailCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Email).MinimumLength(3);
        }
    }
}
