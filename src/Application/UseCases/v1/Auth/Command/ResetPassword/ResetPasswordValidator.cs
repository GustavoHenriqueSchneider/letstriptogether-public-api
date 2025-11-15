using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.v1.Auth.Command.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .SetValidator(new PasswordValidator());
    }
}
