using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using PublicApi.Services.Interfaces;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o GroupMemberController da API interna
/// Rota: /api/v1/groups/{groupId}/members
/// </summary>
[ApiController]
[Route("api/v1/groups/{groupId:guid}/members")]
public class GroupMemberController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public GroupMemberController(IInternalApiService internalApiService)
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
    /// Lista todos os membros do grupo (exceto o próprio usuário)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOtherGroupMembersById(
        [FromRoute] Guid groupId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/members?pageNumber={pageNumber}&pageSize={pageSize}", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Busca informações de um membro específico do grupo
    /// </summary>
    [HttpGet("{memberId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupMemberById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid memberId)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/members/{memberId}", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Remove um membro do grupo (apenas owner pode fazer isso)
    /// </summary>
    [HttpDelete("{memberId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveGroupMemberById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid memberId)
    {
        var token = GetToken();
        var response = await _internalApiService.DeleteAsync(
            $"/api/v1/groups/{groupId}/members/{memberId}", 
            token);
        return await ProcessResponse(response);
    }
}
