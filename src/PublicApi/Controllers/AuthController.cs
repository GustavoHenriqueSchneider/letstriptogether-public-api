using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para endpoints de autenticação da API interna
/// Rotas: /api/v1/auth/email/send, /api/v1/auth/email/validate, /api/v1/auth/register,
/// /api/v1/auth/login, /api/v1/auth/logout, /api/v1/auth/refresh, /api/v1/auth/reset-password/request, /api/v1/auth/reset-password
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public AuthController(IInternalApiService internalApiService)
    {
        _internalApiService = internalApiService;
    }

    /// <summary>
    /// Extrai o token Bearer do header Authorization
    /// </summary>
    private string? GetToken()
    {
        var authHeader = Request.Headers.FirstOrDefault(h => 
            h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return null;
        }
        
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        
        return authHeader.Trim();
    }

    /// <summary>
    /// Processa a resposta da API interna e retorna o resultado apropriado
    /// </summary>
    private async Task<IActionResult> ProcessResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        // Se for 401, retornar com mais informações de debug
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return StatusCode(401, new { 
                error = "Unauthorized", 
                message = "The internal API returned 401. Please check if the endpoint exists and doesn't require authentication.",
                details = content 
            });
        }
        
        // Se for 500, retornar com detalhes do erro
        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            string errorMessage = "A API interna retornou um erro 500. Verifique os logs da API interna para mais detalhes.";
            JsonElement? detailsJson = null;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    using var errorDocument = JsonDocument.Parse(content);
                    var root = errorDocument.RootElement;

                    if (root.TryGetProperty("message", out var message))
                    {
                        errorMessage = message.GetString() ?? errorMessage;
                    }
                    else if (root.TryGetProperty("error", out var error))
                    {
                        errorMessage = error.GetString() ?? errorMessage;
                    }

                    detailsJson = root.Clone();
                }
                catch (JsonException)
                {
                    // usa mensagem padrão
                }
            }
            
            return StatusCode(500, detailsJson.HasValue
                ? new
                {
                    error = "Internal Server Error",
                    message = errorMessage,
                    details = detailsJson.Value
                }
                : new
                {
                    error = "Internal Server Error",
                    message = errorMessage,
                    details = content
                });
        }
        
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }
        
        // Se for JSON, retorna como objeto JSON parseado
        if (!string.IsNullOrEmpty(content))
        {
            try
            {
                // Tenta parsear como JSON
                using var document = JsonDocument.Parse(content);
                var payload = document.RootElement.Clone();
                return StatusCode((int)response.StatusCode, payload);
            }
            catch (JsonException)
            {
                // Se não for JSON válido, retorna como string
                return StatusCode((int)response.StatusCode, content);
            }
        }
        
        return StatusCode((int)response.StatusCode);
    }

    /// <summary>
    /// Etapa 1: Envia código de confirmação por email (registro)
    /// </summary>
    [HttpPost("email/send")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SendRegisterConfirmationEmail([FromBody] object request)
    {
        try
        {
            var response = await _internalApiService.PostAsync("/api/v1/auth/email/send", request, null);
            return await ProcessResponse(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                error = "Internal Server Error", 
                message = $"Erro ao chamar API interna: {ex.Message}",
                details = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// Etapa 2: Valida código de confirmação (registro)
    /// </summary>
    [HttpPost("email/validate")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> ValidateRegisterConfirmationCode([FromBody] object request)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync("/api/v1/auth/email/validate", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Etapa 3: Registra um novo usuário com senha
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Register([FromBody] object request)
    {
        var token = GetToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new { 
                error = "Unauthorized", 
                message = "Token de registro não encontrado. Por favor, volte ao passo anterior."
            });
        }
        
        var response = await _internalApiService.PostAsync("/api/v1/auth/register", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Faz login do usuário
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] object request)
    {
        var response = await _internalApiService.PostAsync("/api/v1/auth/login", request, null);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Revalida o token de acesso usando o refresh token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> RefreshToken([FromBody] object request)
    {
        var response = await _internalApiService.PostAsync("/api/v1/auth/refresh", request, null);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Faz logout do usuário autenticado
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Logout()
    {
        var token = GetToken();

        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new
            {
                error = "Unauthorized",
                message = "Token de acesso não encontrado. Refaça o login para finalizar a sessão."
            });
        }

        var response = await _internalApiService.PostAsync("/api/v1/auth/logout", null, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Solicita reset de senha
    /// </summary>
    [HttpPost("reset-password/request")]
    [ProducesResponseType(202)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RequestResetPassword([FromBody] object request)
    {
        var response = await _internalApiService.PostAsync("/api/v1/auth/reset-password/request", request, null);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Conclui o reset de senha utilizando o token temporário
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ResetPassword([FromBody] object request)
    {
        var token = GetToken();

        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new
            {
                error = "Unauthorized",
                message = "Token de reset de senha não encontrado. Utilize o link recebido por e-mail."
            });
        }

        var response = await _internalApiService.PostAsync("/api/v1/auth/reset-password", request, token);
        return await ProcessResponse(response);
    }
}

