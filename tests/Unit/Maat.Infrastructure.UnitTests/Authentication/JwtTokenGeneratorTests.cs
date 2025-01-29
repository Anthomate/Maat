using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Maat.Domain.Entities;
using Maat.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Maat.Infrastructure.UnitTests.Authentication;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGeneratorTests()
    {
        _jwtSettings = new JwtSettings
        {
            Secret = "super-secret-key-with-at-least-256-bits-for-testing",
            ExpiryMinutes = 60,
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };

        var options = Options.Create(_jwtSettings);
        _jwtTokenGenerator = new JwtTokenGenerator(options);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateValidToken()
    {
        var user = new User("test@example.com", "hashedPassword", "John", "Doe");

        var token = _jwtTokenGenerator.GenerateToken(user);

        token.Should().NotBeNullOrEmpty();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        jwtToken.Should().NotBeNull();
        jwtToken!.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.FirstName);
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == user.LastName);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeCorrectIssuerAndAudience()
    {
        var user = new User("test@example.com", "hashedPassword", "John", "Doe");

        var token = _jwtTokenGenerator.GenerateToken(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        jwtToken.Should().NotBeNull();
        jwtToken!.Header.Should().NotBeNull();
        jwtToken.Payload.Should().NotBeNull();
        jwtToken.Payload.Aud.Should().BeEquivalentTo(new[] { _jwtSettings.Audience });
        jwtToken.Payload.Iss.Should().Be(_jwtSettings.Issuer);
    }

    [Fact]
    public void GenerateToken_ShouldSetCorrectExpiration()
    {
        var user = new User("test@example.com", "hashedPassword", "John", "Doe");
        var expectedExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var token = _jwtTokenGenerator.GenerateToken(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        jwtToken.Should().NotBeNull();
        jwtToken!.ValidTo.Should().BeCloseTo(expectedExpiration, TimeSpan.FromSeconds(10));
    }
}