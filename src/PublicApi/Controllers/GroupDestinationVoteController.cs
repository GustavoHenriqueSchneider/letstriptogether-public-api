using Microsoft.AspNetCore.Mvc;
using PublicApi.DTOs.Requests;
using PublicApi.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o GroupDestinationVoteController da API interna
/// Rota: /api/v1/groups/{groupId}/destination-votes
/// </summary>
[ApiController]
[Route("api/v1/groups/{groupId:guid}/destination-votes")]
public class GroupDestinationVoteController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public GroupDestinationVoteController(IInternalApiService internalApiService)
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
    /// Vota em um destino para o grupo
    /// </summary>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> VoteAtDestinationForGroupId(
        [FromRoute] Guid groupId,
        [FromBody] VoteAtDestinationRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PostAsync(
            $"/api/v1/groups/{groupId}/destination-votes", 
            request, 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Atualiza um voto de destino
    /// </summary>
    [HttpPut("{destinationVoteId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateDestinationVoteById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid destinationVoteId,
        [FromBody] UpdateDestinationVoteRequest request)
    {
        var token = GetToken();
        var response = await _internalApiService.PutAsync(
            $"/api/v1/groups/{groupId}/destination-votes/{destinationVoteId}", 
            request, 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Lista todos os votos de destino do membro no grupo
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupMemberAllDestinationVotesById(
        [FromRoute] Guid groupId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/destination-votes?pageNumber={pageNumber}&pageSize={pageSize}", 
            token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Busca um voto de destino específico por ID
    /// </summary>
    [HttpGet("{destinationVoteId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupDestinationVoteById(
        [FromRoute] Guid groupId,
        [FromRoute] Guid destinationVoteId)
    {
        var token = GetToken();
        var response = await _internalApiService.GetAsync(
            $"/api/v1/groups/{groupId}/destination-votes/{destinationVoteId}", 
            token);
        return await ProcessResponse(response);
    }
}

