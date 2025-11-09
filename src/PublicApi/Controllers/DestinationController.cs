using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using PublicApi.Services.Interfaces;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o DestinationController da API interna
/// Rota: /api/v1/destinations
/// </summary>
[ApiController]
[Route("api/v1/destinations")]
public class DestinationController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public DestinationController(IInternalApiService internalApiService)
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
    /// Busca informações de um destino por ID
    /// </summary>
    /// <param name="destinationId">ID do destino</param>
    /// <returns>Dados do destino</returns>
    [HttpGet("{destinationId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetDestinationById([FromRoute] Guid destinationId)
    {
        var token = GetToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { 
                error = "Invalid token or not found",
                hint = "Make sure you enter the entire token."
            });
        }
        
        var response = await _internalApiService.GetAsync($"/api/v1/destinations/{destinationId}", token);
        return await ProcessResponse(response);
    }
}

