using FluentValidation.TestHelper;
using Maat.Application.Features.Auth.Commands.Register;
using Maat.Domain.Interfaces.Repositories;
using Moq;

namespace Maat.Application.UnitTests.Features.Auth.Commands;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public RegisterCommandValidatorTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _validator = new RegisterCommandValidator(_userRepositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("notanemail")]
    public async Task Email_ShouldHaveValidationError_WhenInvalid(string email)
    {
        var command = new RegisterCommand { Email = email };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("short")]
    [InlineData("onlylowercase123")]
    [InlineData("ONLYUPPERCASE123")]
    [InlineData("NoSpecialChar1")]
    public async Task Password_ShouldHaveValidationError_WhenInvalid(string password)
    {
        var command = new RegisterCommand { Password = password };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task Email_ShouldHaveValidationError_WhenAlreadyExists()
    {
        var command = new RegisterCommand { Email = "existing@example.com" };
        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync(command.Email))
            .ReturnsAsync(true);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Cet email est déjà utilisé");
    }
}