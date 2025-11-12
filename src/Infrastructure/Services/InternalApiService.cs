using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.Login;
using Application.UseCases.Auth.Command.Logout;
using Application.UseCases.Auth.Command.RefreshToken;
using Application.UseCases.Auth.Command.Register;
using Application.UseCases.Auth.Command.RequestResetPassword;
using Application.UseCases.Auth.Command.ResetPassword;
using Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;
using Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;
using Application.UseCases.Destination.Query.GetDestinationById;
using Application.UseCases.Group.Command.CreateGroup;
using Application.UseCases.Group.Command.DeleteGroupById;
using Application.UseCases.Group.Command.LeaveGroupById;
using Application.UseCases.Group.Command.UpdateGroupById;
using Application.UseCases.Group.Query.GetAllGroups;
using Application.UseCases.Group.Query.GetGroupById;
using Application.UseCases.Group.Query.GetNotVotedDestinationsByMemberOnGroup;
using Application.UseCases.GroupDestinationVote.Command.UpdateDestinationVoteById;
using Application.UseCases.GroupDestinationVote.Command.VoteAtDestinationForGroupId;
using Application.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;
using Application.UseCases.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;
using Application.UseCases.GroupInvitation.Command.CancelActiveGroupInvitation;
using Application.UseCases.GroupInvitation.Command.CreateGroupInvitation;
using Application.UseCases.GroupInvitation.Query.GetActiveGroupInvitation;
using Application.UseCases.GroupMatch.Command.RemoveGroupMatchById;
using Application.UseCases.GroupMatch.Query.GetAllGroupMatchesById;
using Application.UseCases.GroupMatch.Query.GetGroupMatchById;
using Application.UseCases.GroupMember.Command.RemoveGroupMemberById;
using Application.UseCases.GroupMember.Query.GetGroupMemberById;
using Application.UseCases.GroupMember.Query.GetOtherGroupMembersById;
using Application.UseCases.Invitation.Command.AcceptInvitation;
using Application.UseCases.Invitation.Command.RefuseInvitation;
using Application.UseCases.User.Command.AnonymizeCurrentUser;
using Application.UseCases.User.Command.DeleteCurrentUser;
using Application.UseCases.User.Command.SetCurrentUserPreferences;
using Application.UseCases.User.Command.UpdateCurrentUser;
using Application.UseCases.User.Query.GetCurrentUser;

namespace Infrastructure.Services;

public class InternalApiService(IHttpClientService httpClientService) : IInternalApiService
{
    // Auth
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

    // User
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

    // Group
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

    // Destination
    public async Task<GetDestinationByIdResponse> GetDestinationByIdAsync(
        GetDestinationByIdQuery request, CancellationToken cancellationToken)
    {
        return await httpClientService.GetAsync<GetDestinationByIdResponse>(
            $"v1/destinations/{request.DestinationId}", cancellationToken);
    }

    // GroupDestinationVote
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

    // GroupInvitation
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
            $"v1/groups/{request.GroupId}/invitations/cancel", null, cancellationToken);
    }

    // GroupMatch
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

    // GroupMember
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

    // Invitation
    public async Task AcceptInvitationAsync(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/invitations/accept", request, cancellationToken);
    }

    public async Task RefuseInvitationAsync(RefuseInvitationCommand request, CancellationToken cancellationToken)
    {
        await httpClientService.PostAsync("v1/invitations/refuse", request, cancellationToken);
    }
}
