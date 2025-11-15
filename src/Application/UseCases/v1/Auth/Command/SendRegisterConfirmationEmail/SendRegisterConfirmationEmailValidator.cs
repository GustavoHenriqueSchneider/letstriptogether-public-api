using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.v1.Auth.Command.SendRegisterConfirmationEmail;

public class SendRegisterConfirmationEmailValidator : AbstractValidator<SendRegisterConfirmationEmailCommand>
{
    public const int NameMaxLength = 150;

    public SendRegisterConfirmationEmailValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .SetValidator(new EmailValidator());
    }
}
