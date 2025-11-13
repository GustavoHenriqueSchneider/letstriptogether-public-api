using FluentValidation;

namespace Application.Common.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 30;

    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .MinimumLength(PasswordMinLength)
            .MaximumLength(PasswordMaxLength)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
            .WithName("Password");
    }
}
