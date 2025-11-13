using MediatR;

namespace Application.UseCases.Auth.Command.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public string Password { get; init; } = null!;
}
