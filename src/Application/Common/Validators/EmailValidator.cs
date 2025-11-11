using FluentValidation;

namespace Application.Common.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public const int EmailMaxLength = 254;

    public EmailValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .MaximumLength(EmailMaxLength)
            .EmailAddress()
            .WithName("Email");
    }
}
