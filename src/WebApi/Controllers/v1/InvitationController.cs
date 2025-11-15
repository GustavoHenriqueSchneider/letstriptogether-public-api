using Application.UseCases.Invitation.Command.AcceptInvitation;
using Application.UseCases.Invitation.Command.RefuseInvitation;
using Application.UseCases.Invitation.Query.GetInvitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/invitations")]
public class InvitationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obter detalhes do convite",
        Description = "Retorna as informações do convite associado ao token informado.")]
    [ProducesResponseType(typeof(GetInvitationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvitation([FromQuery] string token, CancellationToken cancellationToken)
    {
        var query = new GetInvitationQuery
        {
            Token = token
        };
        
        var response = await mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost("accept")]
    [SwaggerOperation(
        Summary = "Aceitar Convite",
        Description = "Aceita um convite de grupo, adicionando o usuário autenticado como membro do grupo.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("refuse")]
    [SwaggerOperation(
        Summary = "Recusar Convite",
        Description = "Recusa um convite de grupo, marcando o convite como recusado.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefuseInvitation([FromBody] RefuseInvitationCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

