using FluentValidation;

namespace Application.UseCases.v1.User.Command.UpdateCurrentUser;

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

