using Microsoft.AspNetCore.Mvc;
using PublicApi.DTOs.Requests;
using PublicApi.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o InvitationController da API interna
/// Rota: /api/v1/invitations
/// </summary>
[ApiController]
[Route("api/v1/invitations")]
public class InvitationController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public InvitationController(IInternalApiService internalApiService)
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
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return StatusCode(401, new
            {
                error = "Unauthorized",
                message = "The internal API returned 401. Please check if the token is valid and not expired.",
                details = content
            });
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return StatusCode((int)response.StatusCode);
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            var payload = document.RootElement.Clone();
            return StatusCode((int)response.StatusCode, payload);
        }
        catch (JsonException)
        {
            return StatusCode((int)response.StatusCode, content);
        }
    }

    /// <summary>
    /// Aceita um convite de grupo
    /// </summary>
    [HttpPost("accept")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync("/api/v1/invitations/accept", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Recusa um convite de grupo
    /// </summary>
    [HttpPost("refuse")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> RefuseInvitation([FromBody] RefuseInvitationRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync("/api/v1/invitations/refuse", request, token);
        return await ProcessResponse(response);
    }
}

