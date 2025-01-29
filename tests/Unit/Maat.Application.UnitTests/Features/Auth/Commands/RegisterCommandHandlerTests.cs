using FluentAssertions;
using Maat.Application.Common.Interfaces;
using Maat.Application.Features.Auth.Commands.Register;
using Maat.Application.UnitTests.Common;
using Maat.Domain.Entities;
using Maat.Domain.Interfaces;
using Maat.Domain.Interfaces.Repositories;
using Moq;

namespace Maat.Application.UnitTests.Features.Auth.Commands;

public class RegisterCommandHandlerTests : TestBase
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldReturnFailure()
    {
        var command = new RegisterCommand 
        { 
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync(command.Email))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Un utilisateur avec cet email existe déjà.");
    }

    [Fact]
    public async Task Handle_WhenValidCommand_ShouldCreateUserAndReturnSuccess()
    {
        var command = new RegisterCommand 
        { 
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        var hashedPassword = "hashedPassword";
        var token = "jwt-token";

        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync(command.Email))
            .ReturnsAsync(false);

        _passwordHasherMock.Setup(x => x.HashPassword(command.Password))
            .Returns(hashedPassword);

        _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(token);

        var result = await _handler.Handle(command, CancellationToken);

        result.Success.Should().BeTrue();
        result.Token.Should().Be(token);

        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u => 
            u.Email == command.Email &&
            u.FirstName == command.FirstName &&
            u.LastName == command.LastName)), Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken), Times.Once);
    }
}