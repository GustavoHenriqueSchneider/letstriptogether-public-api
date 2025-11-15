using FluentValidation;

namespace Application.UseCases.v1.Group.Command.CreateGroup;

public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
{
    public const int NameMaxLength = 30;

    public CreateGroupValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.TripExpectedDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow);
    }
}

