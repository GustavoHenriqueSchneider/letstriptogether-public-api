using Application.UseCases.v1.Auth.Command.Login;
using Application.UseCases.v1.Auth.Command.Logout;
using Application.UseCases.v1.Auth.Command.RefreshToken;
using Application.UseCases.v1.Auth.Command.Register;
using Application.UseCases.v1.Auth.Command.RequestResetPassword;
using Application.UseCases.v1.Auth.Command.ResetPassword;
using Application.UseCases.v1.Auth.Command.SendRegisterConfirmationEmail;
using Application.UseCases.v1.Auth.Command.ValidateRegisterConfirmationCode;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private AuthController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Test]
    public async Task SendRegisterConfirmationEmail_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand { Email = "test@example.com" };
        var response = new SendRegisterConfirmationEmailResponse { Token = "token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<SendRegisterConfirmationEmailCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.SendRegisterConfirmationEmail(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task ValidateRegisterConfirmationCode_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 123456 };
        var response = new ValidateRegisterConfirmationCodeResponse { Token = "token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ValidateRegisterConfirmationCodeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.ValidateRegisterConfirmationCode(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task Register_WhenValid_ShouldReturnCreated()
    {
        // Arrange
        var command = new RegisterCommand { Password = "Password123!", HasAcceptedTermsOfUse = true };
        var response = new RegisterResponse { Id = Guid.NewGuid() };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Register(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task Login_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "Password123!" };
        var response = new LoginResponse { AccessToken = "access", RefreshToken = "refresh" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Login(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task Logout_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Logout(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task RefreshToken_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "refresh-token" };
        var response = new RefreshTokenResponse { AccessToken = "access", RefreshToken = "refresh" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.RefreshToken(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task RequestResetPassword_WhenValid_ShouldReturnAccepted()
    {
        // Arrange
        var command = new RequestResetPasswordCommand { Email = "test@example.com" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RequestResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RequestResetPassword(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AcceptedResult>();
    }

    [Test]
    public async Task ResetPassword_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var command = new ResetPasswordCommand { Password = "NewPassword123!" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ResetPassword(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
