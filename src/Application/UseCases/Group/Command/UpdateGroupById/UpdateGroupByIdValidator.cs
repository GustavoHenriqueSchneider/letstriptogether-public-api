using FluentValidation;

namespace Application.UseCases.Group.Command.UpdateGroupById;

public class UpdateGroupByIdValidator : AbstractValidator<UpdateGroupByIdCommand>
{
    public const int NameMaxLength = 30;

    public UpdateGroupByIdValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.TripExpectedDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.TripExpectedDate.HasValue);
    }
}

