using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.Login;
using Application.UseCases.Auth.Command.Logout;
using Application.UseCases.Auth.Command.RefreshToken;
using Application.UseCases.Auth.Command.Register;
using Application.UseCases.Auth.Command.RequestResetPassword;
using Application.UseCases.Auth.Command.ResetPassword;
using Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;
using Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

namespace Infrastructure.Services;

public class InternalApiService(IHttpClientService httpClientService) : IInternalApiService
{
    public async Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<LoginResponse>("api/v1/auth/login", request, cancellationToken);
    }

    public async Task LogoutAsync(LogoutCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("api/v1/auth/logout", request, cancellationToken);
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<RefreshTokenResponse>("api/v1/auth/refresh", request, cancellationToken);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<RegisterResponse>("api/v1/auth/register", request, cancellationToken);
    }

    public async Task RequestResetPasswordAsync(RequestResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("api/v1/auth/reset-password/request", request, cancellationToken);
    }

    public async Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("api/v1/auth/reset-password", request, cancellationToken);
    }

    public async Task<SendRegisterConfirmationEmailResponse> SendRegisterConfirmationEmailAsync(
        SendRegisterConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<SendRegisterConfirmationEmailResponse>
            ("api/v1/auth/email/send", request, cancellationToken);
    }

    public async Task<ValidateRegisterConfirmationCodeResponse> ValidateRegisterConfirmationCodeAsync(
        ValidateRegisterConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<ValidateRegisterConfirmationCodeResponse>(
            "api/v1/auth/email/validate", request, cancellationToken);
    }
}
