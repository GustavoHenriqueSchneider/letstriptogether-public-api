using MediatR;

namespace Application.UseCases.v1.Auth.Command.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public string Password { get; init; } = null!;
}
