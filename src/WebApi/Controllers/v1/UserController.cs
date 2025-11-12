using Application.UseCases.User.Command.AnonymizeCurrentUser;
using Application.UseCases.User.Command.DeleteCurrentUser;
using Application.UseCases.User.Command.SetCurrentUserPreferences;
using Application.UseCases.User.Command.UpdateCurrentUser;
using Application.UseCases.User.Query.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/users/me")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obter Usuário Atual",
        Description = "Retorna os dados do usuário autenticado, incluindo preferências de viagem.")]
    [ProducesResponseType(typeof(GetCurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Atualizar Usuário Atual",
        Description = "Atualiza as informações do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCurrentUser(
        [FromBody] UpdateCurrentUserCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [SwaggerOperation(
        Summary = "Excluir Usuário Atual",
        Description = "Exclui permanentemente a conta do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCurrentUser(CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCurrentUserCommand(), cancellationToken);
        return NoContent();
    }

    [HttpPatch("anonymize")]
    [SwaggerOperation(
        Summary = "Anonimizar Usuário Atual",
        Description = "Anonimiza os dados pessoais do usuário autenticado, mantendo a conta ativa mas removendo informações identificáveis.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AnonymizeCurrentUser(CancellationToken cancellationToken)
    {
        await mediator.Send(new AnonymizeCurrentUserCommand(), cancellationToken);
        return NoContent();
    }

    [HttpPut("preferences")]
    [SwaggerOperation(
        Summary = "Definir Preferências do Usuário Atual",
        Description = "Define ou atualiza as preferências de viagem do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetCurrentUserPreferences(
        [FromBody] SetCurrentUserPreferencesCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

