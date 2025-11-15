using Application.Common.Interfaces.Services;
using Application.UseCases.Health.Query.GetHealth;
using Application.UseCases.v1.Auth.Command.Login;
using Application.UseCases.v1.Auth.Command.Logout;
using Application.UseCases.v1.Auth.Command.RefreshToken;
using Application.UseCases.v1.Auth.Command.Register;
using Application.UseCases.v1.Auth.Command.RequestResetPassword;
using Application.UseCases.v1.Auth.Command.ResetPassword;
using Application.UseCases.v1.Auth.Command.SendRegisterConfirmationEmail;
using Application.UseCases.v1.Auth.Command.ValidateRegisterConfirmationCode;
using Application.UseCases.v1.Destination.Query.GetDestinationById;
using Application.UseCases.v1.Group.Command.CreateGroup;
using Application.UseCases.v1.Group.Command.DeleteGroupById;
using Application.UseCases.v1.Group.Command.LeaveGroupById;
using Application.UseCases.v1.Group.Command.UpdateGroupById;
using Application.UseCases.v1.Group.Query.GetAllGroups;
using Application.UseCases.v1.Group.Query.GetGroupById;
using Application.UseCases.v1.Group.Query.GetNotVotedDestinationsByMemberOnGroup;
using Application.UseCases.v1.GroupDestinationVote.Command.UpdateDestinationVoteById;
using Application.UseCases.v1.GroupDestinationVote.Command.VoteAtDestinationForGroupId;
using Application.UseCases.v1.GroupDestinationVote.Query.GetGroupDestinationVoteById;
using Application.UseCases.v1.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;
using Application.UseCases.v1.GroupInvitation.Command.CancelActiveGroupInvitation;
using Application.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;
using Application.UseCases.v1.GroupInvitation.Query.GetActiveGroupInvitation;
using Application.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;
using Application.UseCases.v1.GroupMatch.Query.GetAllGroupMatchesById;
using Application.UseCases.v1.GroupMatch.Query.GetGroupMatchById;
using Application.UseCases.v1.GroupMember.Command.RemoveGroupMemberById;
using Application.UseCases.v1.GroupMember.Query.GetGroupMemberById;
using Application.UseCases.v1.GroupMember.Query.GetOtherGroupMembersById;
using Application.UseCases.v1.Invitation.Command.AcceptInvitation;
using Application.UseCases.v1.Invitation.Command.RefuseInvitation;
using Application.UseCases.v1.Invitation.Query.GetInvitation;
using Application.UseCases.v1.User.Command.AnonymizeCurrentUser;
using Application.UseCases.v1.User.Command.ChangeCurrentUserPassword;
using Application.UseCases.v1.User.Command.DeleteCurrentUser;
using Application.UseCases.v1.User.Command.SetCurrentUserPreferences;
using Application.UseCases.v1.User.Command.UpdateCurrentUser;
using Application.UseCases.v1.User.Query.GetCurrentUser;

namespace Infrastructure.Services;

public class InternalApiService(IHttpClientService httpClientService) : IInternalApiService
{
    public async Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<LoginResponse>("v1/auth/login", request, cancellationToken);
    }

    public async Task LogoutAsync(LogoutCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/auth/logout", request, cancellationToken);
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<RefreshTokenResponse>("v1/auth/refresh", request, cancellationToken);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<RegisterResponse>("v1/auth/register", request, cancellationToken);
    }

    public async Task RequestResetPasswordAsync(RequestResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/auth/reset-password/request", request, cancellationToken);
    }

    public async Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/auth/reset-password", request, cancellationToken);
    }

    public async Task<SendRegisterConfirmationEmailResponse> SendRegisterConfirmationEmailAsync(
        SendRegisterConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<SendRegisterConfirmationEmailResponse>
            ("v1/auth/email/send", request, cancellationToken);
    }

    public async Task<ValidateRegisterConfirmationCodeResponse> ValidateRegisterConfirmationCodeAsync(
        ValidateRegisterConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<ValidateRegisterConfirmationCodeResponse>(
            "v1/auth/email/validate", request, cancellationToken);
    }

    public async Task<GetCurrentUserResponse> GetCurrentUserAsync(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetCurrentUserResponse>("v1/users/me", cancellationToken);
    }

    public async Task UpdateCurrentUserAsync(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PutAsync("v1/users/me", request, cancellationToken);
    }

    public async Task DeleteCurrentUserAsync(DeleteCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.DeleteAsync("v1/users/me", cancellationToken);
    }

    public async Task AnonymizeCurrentUserAsync(AnonymizeCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PatchAsync("v1/users/me/anonymize", request, cancellationToken);
    }

    public async Task SetCurrentUserPreferencesAsync(SetCurrentUserPreferencesCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PutAsync("v1/users/me/preferences", request, cancellationToken);
    }

    public async Task ChangeCurrentUserPasswordAsync(ChangeCurrentUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/users/me/change-password", request, cancellationToken);
    }

    public async Task<CreateGroupResponse> CreateGroupAsync(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<CreateGroupResponse>("v1/groups", request, cancellationToken);
    }

    public async Task<GetAllGroupsResponse> GetAllGroupsAsync(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetAllGroupsResponse>(
            $"v1/groups?pageNumber={request.PageNumber}&pageSize={request.PageSize}", cancellationToken);
    }

    public async Task<GetGroupByIdResponse> GetGroupByIdAsync(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetGroupByIdResponse>(
            $"v1/groups/{request.GroupId}", cancellationToken);
    }

    public async Task UpdateGroupByIdAsync(UpdateGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PutAsync($"v1/groups/{request.GroupId}", request, cancellationToken);
    }

    public async Task DeleteGroupByIdAsync(DeleteGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.DeleteAsync($"v1/groups/{request.GroupId}", cancellationToken);
    }

    public async Task LeaveGroupByIdAsync(LeaveGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PatchAsync($"v1/groups/{request.GroupId}/leave", request, cancellationToken);
    }

    public async Task<GetNotVotedDestinationsByMemberOnGroupResponse> GetNotVotedDestinationsByMemberOnGroupAsync(
        GetNotVotedDestinationsByMemberOnGroupQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetNotVotedDestinationsByMemberOnGroupResponse>(
            $"v1/groups/{request.GroupId}/destinations-not-voted?pageNumber={request.PageNumber}&pageSize={request.PageSize}", 
            cancellationToken);
    }

    public async Task<GetDestinationByIdResponse> GetDestinationByIdAsync(
        GetDestinationByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetDestinationByIdResponse>(
            $"v1/destinations/{request.DestinationId}", cancellationToken);
    }

    public async Task<VoteAtDestinationForGroupIdResponse> VoteAtDestinationForGroupIdAsync(
        VoteAtDestinationForGroupIdCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<VoteAtDestinationForGroupIdResponse>(
            $"v1/groups/{request.GroupId}/destination-votes", request, cancellationToken);
    }

    public async Task UpdateDestinationVoteByIdAsync(UpdateDestinationVoteByIdCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PutAsync(
            $"v1/groups/{request.GroupId}/destination-votes/{request.DestinationVoteId}", request, cancellationToken);
    }

    public async Task<GetGroupDestinationVoteByIdResponse> GetGroupDestinationVoteByIdAsync(
        GetGroupDestinationVoteByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetGroupDestinationVoteByIdResponse>(
            $"v1/groups/{request.GroupId}/destination-votes/{request.DestinationVoteId}", cancellationToken);
    }

    public async Task<GetGroupMemberAllDestinationVotesByIdResponse> GetGroupMemberAllDestinationVotesByIdAsync(
        GetGroupMemberAllDestinationVotesByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetGroupMemberAllDestinationVotesByIdResponse>(
            $"v1/groups/{request.GroupId}/destination-votes?pageNumber={request.PageNumber}&pageSize={request.PageSize}", 
            cancellationToken);
    }

    public async Task<CreateGroupInvitationResponse> CreateGroupInvitationAsync(
        CreateGroupInvitationCommand request, CancellationToken cancellationToken)
    {
        return await httpClientService.PostAsync<CreateGroupInvitationResponse>(
            $"v1/groups/{request.GroupId}/invitations", request, cancellationToken);
    }

    public async Task<GetActiveGroupInvitationResponse> GetActiveGroupInvitationAsync(
        GetActiveGroupInvitationQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetActiveGroupInvitationResponse>(
            $"v1/groups/{request.GroupId}/invitations", cancellationToken);
    }

    public async Task CancelActiveGroupInvitationAsync(
        CancelActiveGroupInvitationCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PatchAsync(
            $"v1/groups/{request.GroupId}/invitations/cancel", request, cancellationToken);
    }

    public async Task<GetAllGroupMatchesByIdResponse> GetAllGroupMatchesByIdAsync(
        GetAllGroupMatchesByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetAllGroupMatchesByIdResponse>(
            $"v1/groups/{request.GroupId}/matches?pageNumber={request.PageNumber}&pageSize={request.PageSize}", 
            cancellationToken);
    }

    public async Task<GetGroupMatchByIdResponse> GetGroupMatchByIdAsync(
        GetGroupMatchByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetGroupMatchByIdResponse>(
            $"v1/groups/{request.GroupId}/matches/{request.MatchId}", cancellationToken);
    }

    public async Task RemoveGroupMatchByIdAsync(RemoveGroupMatchByIdCommand request, 
    CancellationToken cancellationToken)
    {
        await httpClientService.DeleteAsync(
            $"v1/groups/{request.GroupId}/matches/{request.MatchId}", cancellationToken);
    }

    public async Task<GetOtherGroupMembersByIdResponse> GetOtherGroupMembersByIdAsync(
        GetOtherGroupMembersByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetOtherGroupMembersByIdResponse>(
            $"v1/groups/{request.GroupId}/members?pageNumber={request.PageNumber}&pageSize={request.PageSize}", 
            cancellationToken);
    }

    public async Task<GetGroupMemberByIdResponse> GetGroupMemberByIdAsync(
        GetGroupMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetGroupMemberByIdResponse>(
            $"v1/groups/{request.GroupId}/members/{request.MemberId}", cancellationToken);
    }

    public async Task RemoveGroupMemberByIdAsync(RemoveGroupMemberByIdCommand request, 
    CancellationToken cancellationToken)
    {
        await httpClientService.DeleteAsync(
            $"v1/groups/{request.GroupId}/members/{request.MemberId}", cancellationToken);
    }

    public async Task AcceptInvitationAsync(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/invitations/accept", request, cancellationToken);
    }

    public async Task RefuseInvitationAsync(RefuseInvitationCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/invitations/refuse", request, cancellationToken);
    }

    public async Task<GetInvitationResponse> GetInvitationAsync(GetInvitationQuery request, CancellationToken cancellationToken)
    {
        var token = Uri.EscapeDataString(request.Token);
        return await httpClientService.GetAsync<GetInvitationResponse>($"v1/invitations?token={token}", cancellationToken);
    }

    public async Task<GetHealthResponse> GetHealthAsync(CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetHealthResponse>("health", cancellationToken);
    }
}
