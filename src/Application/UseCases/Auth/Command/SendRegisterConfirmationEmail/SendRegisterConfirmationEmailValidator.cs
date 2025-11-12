using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;

public class SendRegisterConfirmationEmailValidator : AbstractValidator<SendRegisterConfirmationEmailCommand>
{
    public const int NameMaxLength = 150;

    public SendRegisterConfirmationEmailValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
        
        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());
    }
}
