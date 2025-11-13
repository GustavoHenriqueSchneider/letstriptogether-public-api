using FluentValidation;

namespace Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

public class ValidateRegisterConfirmationCodeValidator : AbstractValidator<ValidateRegisterConfirmationCodeCommand>
{
    public const int CodeMinValue = 100000;
    public const int CodeMaxValue = 999999;

    public ValidateRegisterConfirmationCodeValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .GreaterThanOrEqualTo(CodeMinValue)
            .LessThanOrEqualTo(CodeMaxValue);
    }
}
