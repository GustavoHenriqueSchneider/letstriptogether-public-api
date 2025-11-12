using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;

public class SendRegisterConfirmationEmailValidator : AbstractValidator<SendRegisterConfirmationEmailCommand>
{
    public SendRegisterConfirmationEmailValidator()
    {
        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());
    }
}
