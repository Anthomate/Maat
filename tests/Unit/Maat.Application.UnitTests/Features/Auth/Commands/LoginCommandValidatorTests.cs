using FluentValidation.TestHelper;
using Maat.Application.Features.Auth.Commands.Login;

namespace Maat.Application.UnitTests.Features.Auth.Commands;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("notanemail")]
    public async Task Email_ShouldHaveValidationError_WhenInvalid(string email)
    {
        var command = new LoginCommand { Email = email };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Password_ShouldHaveValidationError_WhenEmpty(string password)
    {
        var command = new LoginCommand { Password = password };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}