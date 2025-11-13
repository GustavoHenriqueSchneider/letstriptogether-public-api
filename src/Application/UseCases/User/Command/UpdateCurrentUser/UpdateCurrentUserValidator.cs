using FluentValidation;

namespace Application.UseCases.User.Command.UpdateCurrentUser;

public class UpdateCurrentUserValidator : AbstractValidator<UpdateCurrentUserCommand>
{
    public const int NameMaxLength = 150;

    public UpdateCurrentUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}

