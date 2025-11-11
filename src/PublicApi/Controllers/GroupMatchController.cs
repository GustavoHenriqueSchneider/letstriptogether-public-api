using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using PublicApi.Services.Interfaces;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o GroupMatchController da API interna
/// Rota: /api/v1/groups/{groupId}/matches
/// </summary>
[ApiController]
[Route("api/v1/groups/{groupId:guid}/matches")]
public class GroupMatchController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public GroupMatchController(IInternalApiService internalApiService)
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
    /// Lista todos os matches do grupo
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAllGroupMatchesById(
        [FromRoute] Guid groupId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/matches?pageNumber={pageNumber}&pageSize={pageSize}", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Busca um match específico por ID
    /// </summary>
    [HttpGet("{matchId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupMatchById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid matchId)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/matches/{matchId}", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Remove um match do grupo
    /// </summary>
    [HttpDelete("{matchId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveGroupMatchById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid matchId)
    {
        var token = GetToken();
        var response = await _internalApiService.DeleteAsync(
            $"/api/v1/groups/{groupId}/matches/{matchId}", 
            token);
        return await ProcessResponse(response);
    }
}

