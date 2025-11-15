using FluentValidation;

namespace Application.UseCases.Invitation.Query.GetInvitation;

public class GetInvitationValidator : AbstractValidator<GetInvitationQuery>
{
    public GetInvitationValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
