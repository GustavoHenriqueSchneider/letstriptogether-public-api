using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.Auth.Command.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => x.HasAcceptedTermsOfUse)
            .Must(x => x).WithMessage("Terms of use must be accepted for user registration.");
    }
}
