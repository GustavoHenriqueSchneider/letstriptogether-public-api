using Microsoft.AspNetCore.Mvc;
using PublicApi.DTOs.Requests;
using PublicApi.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o GroupController da API interna
/// Rota: /api/v1/groups
/// </summary>
[ApiController]
[Route("api/v1/groups")]
public class GroupController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public GroupController(IInternalApiService internalApiService)
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
    /// Cria um novo grupo
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync("/api/v1/groups", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Lista todos os grupos do usuário atual
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetAllGroups([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync($"/api/v1/groups?pageNumber={pageNumber}&pageSize={pageSize}", token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Busca informações de um grupo por ID
    /// </summary>
    [HttpGet("{groupId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupById([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync($"/api/v1/groups/{groupId}", token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Atualiza informações de um grupo
    /// </summary>
    [HttpPut("{groupId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateGroupById([FromRoute] Guid groupId, [FromBody] UpdateGroupRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PutAsync($"/api/v1/groups/{groupId}", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Deleta um grupo
    /// </summary>
    [HttpDelete("{groupId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteGroupById([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.DeleteAsync($"/api/v1/groups/{groupId}", token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Remove o usuário atual do grupo
    /// </summary>
    [HttpPatch("{groupId:guid}/leave")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> LeaveGroupById([FromRoute] Guid groupId)
    {
        var token = GetToken();
        var response = await _internalApiService.PatchAsync($"/api/v1/groups/{groupId}/leave", null, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Lista destinos não votados pelo membro no grupo
    /// </summary>
    [HttpGet("{groupId:guid}/destinations-not-voted")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetNotVotedDestinationsByMemberOnGroup(
        [FromRoute] Guid groupId,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/destinations-not-voted?pageNumber={pageNumber}&pageSize={pageSize}", 
            token);
        return await ProcessResponse(response);
    }
}

