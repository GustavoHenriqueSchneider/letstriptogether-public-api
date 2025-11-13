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
using DestinationAttractionModel = Application.UseCases.Destination.Query.GetDestinationById.DestinationAttractionModel;
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
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;

namespace Infrastructure.UnitTests.Services;

[TestFixture]
public class InternalApiServiceTests
{
    private Mock<IHttpClientService> _httpClientServiceMock = null!;
    private InternalApiService _internalApiService = null!;

    [SetUp]
    public void SetUp()
    {
        _httpClientServiceMock = new Mock<IHttpClientService>();
        _internalApiService = new InternalApiService(_httpClientServiceMock.Object);
    }

    #region Auth Tests

    [Test]
    public async Task LoginAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@test.com", Password = "Password123!" };
        var expectedResponse = new LoginResponse { AccessToken = "token", RefreshToken = "refresh" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<LoginResponse>("v1/auth/login", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.LoginAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<LoginResponse>("v1/auth/login", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task LogoutAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new LogoutCommand();

        _httpClientServiceMock
            .Setup(x => x.PostAsync("v1/auth/logout", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.LogoutAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync("v1/auth/logout", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RefreshTokenAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "refresh" };
        var expectedResponse = new RefreshTokenResponse { AccessToken = "token", RefreshToken = "refresh" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<RefreshTokenResponse>("v1/auth/refresh", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.RefreshTokenAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<RefreshTokenResponse>("v1/auth/refresh", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RegisterAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new RegisterCommand { Password = "Password123!", HasAcceptedTermsOfUse = true };
        var expectedResponse = new RegisterResponse { Id = Guid.NewGuid() };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<RegisterResponse>("v1/auth/register", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.RegisterAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<RegisterResponse>("v1/auth/register", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RequestResetPasswordAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new RequestResetPasswordCommand { Email = "test@test.com" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync("v1/auth/reset-password/request", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.RequestResetPasswordAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync("v1/auth/reset-password/request", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ResetPasswordAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new ResetPasswordCommand { Password = "NewPassword123!" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync("v1/auth/reset-password", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.ResetPasswordAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync("v1/auth/reset-password", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task SendRegisterConfirmationEmailAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand { Email = "test@test.com", Name = "Test" };
        var expectedResponse = new SendRegisterConfirmationEmailResponse { Token = "token" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<SendRegisterConfirmationEmailResponse>(
                "v1/auth/email/send", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.SendRegisterConfirmationEmailAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<SendRegisterConfirmationEmailResponse>(
                "v1/auth/email/send", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ValidateRegisterConfirmationCodeAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 123456 };
        var expectedResponse = new ValidateRegisterConfirmationCodeResponse { Token = "token" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<ValidateRegisterConfirmationCodeResponse>(
                "v1/auth/email/validate", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.ValidateRegisterConfirmationCodeAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<ValidateRegisterConfirmationCodeResponse>(
                "v1/auth/email/validate", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region User Tests

    [Test]
    public async Task GetCurrentUserAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var query = new GetCurrentUserQuery();
        var expectedResponse = new GetCurrentUserResponse 
        { 
            Name = "Test User", 
            Email = "test@test.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetCurrentUserResponse>("v1/users/me", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetCurrentUserAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetCurrentUserResponse>("v1/users/me", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateCurrentUserAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand { Name = "New Name" };

        _httpClientServiceMock
            .Setup(x => x.PutAsync("v1/users/me", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.UpdateCurrentUserAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PutAsync("v1/users/me", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteCurrentUserAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new DeleteCurrentUserCommand();

        _httpClientServiceMock
            .Setup(x => x.DeleteAsync("v1/users/me", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.DeleteCurrentUserAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.DeleteAsync("v1/users/me", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task AnonymizeCurrentUserAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new AnonymizeCurrentUserCommand();

        _httpClientServiceMock
            .Setup(x => x.PatchAsync("v1/users/me/anonymize", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.AnonymizeCurrentUserAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PatchAsync("v1/users/me/anonymize", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task SetCurrentUserPreferencesAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand 
        { 
            LikesCommercial = false,
            Food = new List<string> { "Italian" },
            Culture = new List<string> { "Museums" },
            Entertainment = new List<string> { "Concerts" },
            PlaceTypes = new List<string> { "Beach" }
        };

        _httpClientServiceMock
            .Setup(x => x.PutAsync("v1/users/me/preferences", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.SetCurrentUserPreferencesAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PutAsync("v1/users/me/preferences", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Group Tests

    [Test]
    public async Task CreateGroupAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new CreateGroupCommand 
        { 
            Name = "Test Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
        };
        var expectedResponse = new CreateGroupResponse { Id = Guid.NewGuid() };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<CreateGroupResponse>("v1/groups", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.CreateGroupAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<CreateGroupResponse>("v1/groups", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetAllGroupsAsync_ShouldCallHttpClientServiceWithQueryParameters()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 1, PageSize = 10 };
        var expectedResponse = new GetAllGroupsResponse 
        { 
            Data = new List<GetAllGroupsResponseData>(),
            Hits = 0
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetAllGroupsResponse>(
                "v1/groups?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetAllGroupsAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetAllGroupsResponse>(
                "v1/groups?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetGroupByIdAsync_ShouldCallHttpClientServiceWithGroupId()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetGroupByIdQuery { GroupId = groupId };
        var expectedResponse = new GetGroupByIdResponse 
        { 
            Name = "Test Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            Preferences = new GetGroupByIdPreferenceResponse
            {
                LikesCommercial = true,
                Food = new List<string>(),
                Culture = new List<string>(),
                Entertainment = new List<string>(),
                PlaceTypes = new List<string>()
            }
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetGroupByIdResponse>(
                $"v1/groups/{groupId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetGroupByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetGroupByIdResponse>(
                $"v1/groups/{groupId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateGroupByIdAsync_ShouldCallHttpClientServiceWithGroupId()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new UpdateGroupByIdCommand { GroupId = groupId, Name = "Updated Name" };

        _httpClientServiceMock
            .Setup(x => x.PutAsync($"v1/groups/{groupId}", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.UpdateGroupByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PutAsync($"v1/groups/{groupId}", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteGroupByIdAsync_ShouldCallHttpClientServiceWithGroupId()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new DeleteGroupByIdCommand { GroupId = groupId };

        _httpClientServiceMock
            .Setup(x => x.DeleteAsync($"v1/groups/{groupId}", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.DeleteGroupByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.DeleteAsync($"v1/groups/{groupId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task LeaveGroupByIdAsync_ShouldCallHttpClientServiceWithGroupId()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new LeaveGroupByIdCommand { GroupId = groupId };

        _httpClientServiceMock
            .Setup(x => x.PatchAsync($"v1/groups/{groupId}/leave", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.LeaveGroupByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PatchAsync($"v1/groups/{groupId}/leave", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetNotVotedDestinationsByMemberOnGroupAsync_ShouldCallHttpClientServiceWithQueryParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery 
        { 
            GroupId = groupId, 
            PageNumber = 1, 
            PageSize = 10 
        };
        var expectedResponse = new GetNotVotedDestinationsByMemberOnGroupResponse 
        { 
            Data = new List<GetNotVotedDestinationsByMemberOnGroupResponseData>(),
            Hits = 0
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetNotVotedDestinationsByMemberOnGroupResponse>(
                $"v1/groups/{groupId}/destinations-not-voted?pageNumber=1&pageSize=10", 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetNotVotedDestinationsByMemberOnGroupAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetNotVotedDestinationsByMemberOnGroupResponse>(
                $"v1/groups/{groupId}/destinations-not-voted?pageNumber=1&pageSize=10", 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Destination Tests

    [Test]
    public async Task GetDestinationByIdAsync_ShouldCallHttpClientServiceWithDestinationId()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        var query = new GetDestinationByIdQuery { DestinationId = destinationId };
        var expectedResponse = new GetDestinationByIdResponse 
        { 
            Place = "Test Place",
            Description = "Test Description",
            Attractions = new List<DestinationAttractionModel>(),
            CreatedAt = DateTime.UtcNow
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetDestinationByIdResponse>(
                $"v1/destinations/{destinationId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetDestinationByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetDestinationByIdResponse>(
                $"v1/destinations/{destinationId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GroupDestinationVote Tests

    [Test]
    public async Task VoteAtDestinationForGroupIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new VoteAtDestinationForGroupIdCommand
        {
            GroupId = groupId,
            DestinationId = Guid.NewGuid(),
            IsApproved = true
        };
        var expectedResponse = new VoteAtDestinationForGroupIdResponse { Id = Guid.NewGuid() };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<VoteAtDestinationForGroupIdResponse>(
                $"v1/groups/{groupId}/destination-votes", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.VoteAtDestinationForGroupIdAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<VoteAtDestinationForGroupIdResponse>(
                $"v1/groups/{groupId}/destination-votes", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateDestinationVoteByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var destinationVoteId = Guid.NewGuid();
        var command = new UpdateDestinationVoteByIdCommand
        {
            GroupId = groupId,
            DestinationVoteId = destinationVoteId,
            IsApproved = true
        };

        _httpClientServiceMock
            .Setup(x => x.PutAsync(
                $"v1/groups/{groupId}/destination-votes/{destinationVoteId}", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.UpdateDestinationVoteByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PutAsync(
                $"v1/groups/{groupId}/destination-votes/{destinationVoteId}", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetGroupDestinationVoteByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var destinationVoteId = Guid.NewGuid();
        var query = new GetGroupDestinationVoteByIdQuery
        {
            GroupId = groupId,
            DestinationVoteId = destinationVoteId
        };
        var expectedResponse = new GetGroupDestinationVoteByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetGroupDestinationVoteByIdResponse>(
                $"v1/groups/{groupId}/destination-votes/{destinationVoteId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetGroupDestinationVoteByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetGroupDestinationVoteByIdResponse>(
                $"v1/groups/{groupId}/destination-votes/{destinationVoteId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetGroupMemberAllDestinationVotesByIdAsync_ShouldCallHttpClientServiceWithQueryParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = groupId,
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetGroupMemberAllDestinationVotesByIdResponse
        {
            Data = new List<GetGroupMemberAllDestinationVotesByIdResponseData>(),
            Hits = 0
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetGroupMemberAllDestinationVotesByIdResponse>(
                $"v1/groups/{groupId}/destination-votes?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetGroupMemberAllDestinationVotesByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetGroupMemberAllDestinationVotesByIdResponse>(
                $"v1/groups/{groupId}/destination-votes?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GroupInvitation Tests

    [Test]
    public async Task CreateGroupInvitationAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new CreateGroupInvitationCommand { GroupId = groupId };
        var expectedResponse = new CreateGroupInvitationResponse { Token = "invitation-token" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync<CreateGroupInvitationResponse>(
                $"v1/groups/{groupId}/invitations", command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.CreateGroupInvitationAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.PostAsync<CreateGroupInvitationResponse>(
                $"v1/groups/{groupId}/invitations", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetActiveGroupInvitationAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetActiveGroupInvitationQuery { GroupId = groupId };
        var expectedResponse = new GetActiveGroupInvitationResponse { Token = "active-token" };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetActiveGroupInvitationResponse>(
                $"v1/groups/{groupId}/invitations", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetActiveGroupInvitationAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetActiveGroupInvitationResponse>(
                $"v1/groups/{groupId}/invitations", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task CancelActiveGroupInvitationAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new CancelActiveGroupInvitationCommand { GroupId = groupId };

        _httpClientServiceMock
            .Setup(x => x.PatchAsync(
                $"v1/groups/{groupId}/invitations/cancel", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.CancelActiveGroupInvitationAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PatchAsync(
                $"v1/groups/{groupId}/invitations/cancel", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GroupMatch Tests

    [Test]
    public async Task GetAllGroupMatchesByIdAsync_ShouldCallHttpClientServiceWithQueryParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetAllGroupMatchesByIdQuery
        {
            GroupId = groupId,
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetAllGroupMatchesByIdResponse
        {
            Data = new List<GetAllGroupMatchesByIdResponseData>(),
            Hits = 0
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetAllGroupMatchesByIdResponse>(
                $"v1/groups/{groupId}/matches?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetAllGroupMatchesByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetAllGroupMatchesByIdResponse>(
                $"v1/groups/{groupId}/matches?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetGroupMatchByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var query = new GetGroupMatchByIdQuery
        {
            GroupId = groupId,
            MatchId = matchId
        };
        var expectedResponse = new GetGroupMatchByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetGroupMatchByIdResponse>(
                $"v1/groups/{groupId}/matches/{matchId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetGroupMatchByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetGroupMatchByIdResponse>(
                $"v1/groups/{groupId}/matches/{matchId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RemoveGroupMatchByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var command = new RemoveGroupMatchByIdCommand
        {
            GroupId = groupId,
            MatchId = matchId
        };

        _httpClientServiceMock
            .Setup(x => x.DeleteAsync(
                $"v1/groups/{groupId}/matches/{matchId}", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.RemoveGroupMatchByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.DeleteAsync(
                $"v1/groups/{groupId}/matches/{matchId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GroupMember Tests

    [Test]
    public async Task GetOtherGroupMembersByIdAsync_ShouldCallHttpClientServiceWithQueryParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var query = new GetOtherGroupMembersByIdQuery
        {
            GroupId = groupId,
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetOtherGroupMembersByIdResponse
        {
            Data = new List<GetOtherGroupMembersByIdResponseData>(),
            Hits = 0
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetOtherGroupMembersByIdResponse>(
                $"v1/groups/{groupId}/members?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetOtherGroupMembersByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetOtherGroupMembersByIdResponse>(
                $"v1/groups/{groupId}/members?pageNumber=1&pageSize=10", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetGroupMemberByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var query = new GetGroupMemberByIdQuery
        {
            GroupId = groupId,
            MemberId = memberId
        };
        var expectedResponse = new GetGroupMemberByIdResponse
        {
            Name = "Test Member",
            IsOwner = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _httpClientServiceMock
            .Setup(x => x.GetAsync<GetGroupMemberByIdResponse>(
                $"v1/groups/{groupId}/members/{memberId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _internalApiService.GetGroupMemberByIdAsync(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.GetAsync<GetGroupMemberByIdResponse>(
                $"v1/groups/{groupId}/members/{memberId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RemoveGroupMemberByIdAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var command = new RemoveGroupMemberByIdCommand
        {
            GroupId = groupId,
            MemberId = memberId
        };

        _httpClientServiceMock
            .Setup(x => x.DeleteAsync(
                $"v1/groups/{groupId}/members/{memberId}", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.RemoveGroupMemberByIdAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.DeleteAsync(
                $"v1/groups/{groupId}/members/{memberId}", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Invitation Tests

    [Test]
    public async Task AcceptInvitationAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new AcceptInvitationCommand { Token = "invitation-token" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync(
                "v1/invitations/accept", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.AcceptInvitationAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync(
                "v1/invitations/accept", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RefuseInvitationAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = "invitation-token" };

        _httpClientServiceMock
            .Setup(x => x.PostAsync(
                "v1/invitations/refuse", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.RefuseInvitationAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync(
                "v1/invitations/refuse", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
