using Application.UseCases.Auth.Command.Login;
using Application.UseCases.Auth.Command.Logout;
using Application.UseCases.Auth.Command.RefreshToken;
using Application.UseCases.Auth.Command.Register;
using Application.UseCases.Auth.Command.RequestResetPassword;
using Application.UseCases.Auth.Command.ResetPassword;
using Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;
using Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

namespace Application.Common.Interfaces.Services;

public interface IInternalApiService
{
    Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
    Task LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenCommand request, CancellationToken cancellationToken);
    Task<RegisterResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken);
    Task RequestResetPasswordAsync(RequestResetPasswordCommand request, CancellationToken cancellationToken);
    Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken);
    Task<SendRegisterConfirmationEmailResponse> SendRegisterConfirmationEmailAsync(
        SendRegisterConfirmationEmailCommand request, CancellationToken cancellationToken);
    Task<ValidateRegisterConfirmationCodeResponse> ValidateRegisterConfirmationCodeAsync(
        ValidateRegisterConfirmationCodeCommand request, CancellationToken cancellationToken);
}

