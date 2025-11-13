using FluentValidation;

namespace Application.Common.Validators;

public class CulturePreferencesValidator : AbstractValidator<IEnumerable<string>>
{
    public CulturePreferencesValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleForEach(x => x)
            .NotEmpty();
    }
}

public class EntertainmentPreferencesValidator : AbstractValidator<IEnumerable<string>>
{
    public EntertainmentPreferencesValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleForEach(x => x)
            .NotEmpty();
    }
}

public class PlaceTypePreferencesValidator : AbstractValidator<IEnumerable<string>>
{
    public PlaceTypePreferencesValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleForEach(x => x)
            .NotEmpty();
    }
}
