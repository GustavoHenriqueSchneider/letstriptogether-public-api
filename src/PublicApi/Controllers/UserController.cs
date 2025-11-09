using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using PublicApi.Services.Interfaces;

namespace PublicApi.Controllers;

/// <summary>
/// Controller que faz proxy para o UserController da API interna
/// Rota: /api/v1/users/me
/// </summary>
[ApiController]
[Route("api/v1/users/me")]
public class UserController : ControllerBase
{
    private readonly IInternalApiService _internalApiService;

    public UserController(IInternalApiService internalApiService)
    {
        _internalApiService = internalApiService;
    }

    /// <summary>
    /// Extrai o token Bearer do header Authorization
    /// </summary>
    private string? GetToken()
    {
        // Tenta pegar do header Authorization (case-insensitive)
        var authHeader = Request.Headers.FirstOrDefault(h => 
            h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return null;
        }
        
        // Remove "Bearer " se existir (case-insensitive)
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        
        // Se não tiver "Bearer ", retorna o token direto (pode ser que já venha sem o prefixo)
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
    /// Busca informações do usuário atual autenticado
    /// </summary>
    /// <returns>Dados do usuário atual (nome, email, preferências)</returns>
    [HttpGet(Name = "GetCurrentUser")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var token = GetToken();
        
        // Debug: verificar se o token está sendo extraído corretamente
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { 
                error = "Invalid token or not found",
                hint = "Make sure you enter the entire token."
            });
        }
        
        var response = await _internalApiService.GetAsync("/api/v1/users/me", token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Atualiza nome e informações básicas do usuário atual
    /// </summary>
    /// <param name="request">Dados para atualização (nome)</param>
    /// <returns>Sem conteúdo (204) se sucesso</returns>
    [HttpPut(Name = "UpdateCurrentUser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] object request)
    {
        var token = GetToken();
        var response = await _internalApiService.PutAsync("/api/v1/users/me", request, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Deleta permanentemente a conta do usuário atual
    /// </summary>
    /// <returns>Sem conteúdo (204) se sucesso</returns>
    [HttpDelete(Name = "DeleteCurrentUser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCurrentUser()
    {
        var token = GetToken();
        var response = await _internalApiService.DeleteAsync("/api/v1/users/me", token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Anonimiza a conta do usuário atual (remove dados pessoais mas mantém a conta)
    /// </summary>
    /// <returns>Sem conteúdo (204) se sucesso</returns>
    [HttpPatch("anonymize")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AnonymizeCurrentUser()
    {
        var token = GetToken();
        var response = await _internalApiService.PatchAsync("/api/v1/users/me/anonymize", null, token);
        return await ProcessResponse(response);
    }

    /// <summary>
    /// Atualiza as preferências do usuário atual (categorias, etc.)
    /// </summary>
    /// <param name="request">Preferências do usuário</param>
    /// <returns>Sem conteúdo (204) se sucesso</returns>
    [HttpPut("preferences")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> SetCurrentUserPreferences([FromBody] object request)
    {
        var token = GetToken();
        var response = await _internalApiService.PutAsync("/api/v1/users/me/preferences", request, token);
        return await ProcessResponse(response);
    }
}
