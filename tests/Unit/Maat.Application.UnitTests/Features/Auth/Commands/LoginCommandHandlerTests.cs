using FluentAssertions;
using Maat.Application.Common.Interfaces;
using Maat.Application.Features.Auth.Commands.Login;
using Maat.Application.UnitTests.Common;
using Maat.Domain.Entities;
using Maat.Domain.Interfaces.Repositories;
using Moq;

namespace Maat.Application.UnitTests.Features.Auth.Commands;

public class LoginCommandHandlerTests : TestBase
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldReturnFailure()
    {
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "password"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User)null);

        var result = await _handler.Handle(command, CancellationToken);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Identifiants invalides.");
    }

    [Fact]
    public async Task Handle_WhenPasswordIncorrect_ShouldReturnFailure()
    {
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var user = new User("test@example.com", "hashedPassword", "John", "Doe");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(false);

        var result = await _handler.Handle(command, CancellationToken);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Identifiants invalides.");
    }

    [Fact]
    public async Task Handle_WhenValidCredentials_ShouldReturnSuccess()
    {
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "password"
        };

        var user = new User("test@example.com", "hashedPassword", "John", "Doe");
        var token = "jwt-token";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(user))
            .Returns(token);

        var result = await _handler.Handle(command, CancellationToken);

        result.Success.Should().BeTrue();
        result.Token.Should().Be(token);
    }
}