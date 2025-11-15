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
using Application.UseCases.Invitation.Query.GetInvitation;
using Application.UseCases.User.Command.AnonymizeCurrentUser;
using Application.UseCases.User.Command.ChangeCurrentUserPassword;
using Application.UseCases.User.Command.DeleteCurrentUser;
using Application.UseCases.User.Command.SetCurrentUserPreferences;
using Application.UseCases.User.Command.UpdateCurrentUser;
using Application.UseCases.User.Query.GetCurrentUser;

namespace Application.Common.Interfaces.Services;

public interface IInternalApiService
{
    Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
    Task LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenCommand request, CancellationToken cancellationToken);
    Task<RegisterResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken);
    Task RequestResetPasswordAsync(RequestResetPasswordCommand request, CancellationToken cancellationToken);
    Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken);
    Task<SendRegisterConfirmationEmailResponse> SendRegisterConfirmationEmailAsync(
        SendRegisterConfirmationEmailCommand request, CancellationToken cancellationToken);
    Task<ValidateRegisterConfirmationCodeResponse> ValidateRegisterConfirmationCodeAsync(
        ValidateRegisterConfirmationCodeCommand request, CancellationToken cancellationToken);
    Task<GetCurrentUserResponse> GetCurrentUserAsync(GetCurrentUserQuery request, CancellationToken cancellationToken);
    Task UpdateCurrentUserAsync(UpdateCurrentUserCommand request, CancellationToken cancellationToken);
    Task DeleteCurrentUserAsync(DeleteCurrentUserCommand request, CancellationToken cancellationToken);
    Task AnonymizeCurrentUserAsync(AnonymizeCurrentUserCommand request, CancellationToken cancellationToken);
    Task SetCurrentUserPreferencesAsync(SetCurrentUserPreferencesCommand request, CancellationToken cancellationToken);
    Task ChangeCurrentUserPasswordAsync(ChangeCurrentUserPasswordCommand request, CancellationToken cancellationToken);
    Task<CreateGroupResponse> CreateGroupAsync(CreateGroupCommand request, CancellationToken cancellationToken);
    Task<GetAllGroupsResponse> GetAllGroupsAsync(GetAllGroupsQuery request, CancellationToken cancellationToken);
    Task<GetGroupByIdResponse> GetGroupByIdAsync(GetGroupByIdQuery request, CancellationToken cancellationToken);
    Task UpdateGroupByIdAsync(UpdateGroupByIdCommand request, CancellationToken cancellationToken);
    Task DeleteGroupByIdAsync(DeleteGroupByIdCommand request, CancellationToken cancellationToken);
    Task LeaveGroupByIdAsync(LeaveGroupByIdCommand request, CancellationToken cancellationToken);
    Task<GetNotVotedDestinationsByMemberOnGroupResponse> GetNotVotedDestinationsByMemberOnGroupAsync(
        GetNotVotedDestinationsByMemberOnGroupQuery request, CancellationToken cancellationToken);
    Task<GetDestinationByIdResponse> GetDestinationByIdAsync(GetDestinationByIdQuery request, CancellationToken cancellationToken);
    Task<VoteAtDestinationForGroupIdResponse> VoteAtDestinationForGroupIdAsync(
        VoteAtDestinationForGroupIdCommand request, CancellationToken cancellationToken);
    Task UpdateDestinationVoteByIdAsync(UpdateDestinationVoteByIdCommand request, CancellationToken cancellationToken);
    Task<GetGroupDestinationVoteByIdResponse> GetGroupDestinationVoteByIdAsync(
        GetGroupDestinationVoteByIdQuery request, CancellationToken cancellationToken);
    Task<GetGroupMemberAllDestinationVotesByIdResponse> GetGroupMemberAllDestinationVotesByIdAsync(
        GetGroupMemberAllDestinationVotesByIdQuery request, CancellationToken cancellationToken);
    Task<CreateGroupInvitationResponse> CreateGroupInvitationAsync(
        CreateGroupInvitationCommand request, CancellationToken cancellationToken);
    Task<GetActiveGroupInvitationResponse> GetActiveGroupInvitationAsync(
        GetActiveGroupInvitationQuery request, CancellationToken cancellationToken);
    Task CancelActiveGroupInvitationAsync(
        CancelActiveGroupInvitationCommand request, CancellationToken cancellationToken);
    Task<GetAllGroupMatchesByIdResponse> GetAllGroupMatchesByIdAsync(
        GetAllGroupMatchesByIdQuery request, CancellationToken cancellationToken);
    Task<GetGroupMatchByIdResponse> GetGroupMatchByIdAsync(
        GetGroupMatchByIdQuery request, CancellationToken cancellationToken);
    Task RemoveGroupMatchByIdAsync(RemoveGroupMatchByIdCommand request, CancellationToken cancellationToken);
    Task<GetOtherGroupMembersByIdResponse> GetOtherGroupMembersByIdAsync(
        GetOtherGroupMembersByIdQuery request, CancellationToken cancellationToken);
    Task<GetGroupMemberByIdResponse> GetGroupMemberByIdAsync(
        GetGroupMemberByIdQuery request, CancellationToken cancellationToken);
    Task RemoveGroupMemberByIdAsync(RemoveGroupMemberByIdCommand request, CancellationToken cancellationToken);
    Task AcceptInvitationAsync(AcceptInvitationCommand request, CancellationToken cancellationToken);
    Task RefuseInvitationAsync(RefuseInvitationCommand request, CancellationToken cancellationToken);
    Task<GetInvitationResponse> GetInvitationAsync(GetInvitationQuery request, CancellationToken cancellationToken);
}

