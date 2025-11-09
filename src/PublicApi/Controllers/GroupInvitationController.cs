using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using PublicApi.Services.Interfaces;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o GroupInvitationController da API interna
/// Rota: /api/v1/groups/{groupId}/invitations
/// </summary>
[ApiController]
[Route("api/v1/groups/{groupId:guid}/invitations")]
public class GroupInvitationController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public GroupInvitationController(IInternalApiService internalApiService)
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
    /// Cria um novo convite para o grupo
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateGroupInvitation([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync(
            $"/api/v1/groups/{groupId}/invitations", 
            null, 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Busca o convite ativo do grupo
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetActiveGroupInvitation([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/invitations", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Cancela o convite ativo do grupo
    /// </summary>
    [HttpPatch("cancel")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CancelActiveGroupInvitation([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.PatchAsync(
            $"/api/v1/groups/{groupId}/invitations/cancel", 
            null, 
            token);
        return await ProcessResponse(response);
    }
}

