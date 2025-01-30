using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Maat.Api.IntegrationTests;
using Maat.Api.IntegrationTests.Common;
using Maat.Application.Features.Auth.Commands.Login;
using Maat.Application.Features.Auth.Commands.Register;
using Maat.Service;
using Maat.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(WebApplicationFactory<Program> factory) 
        : base(factory)
    {
    }
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        await _dbContext.Database.EnsureCreatedAsync();
        
        await ClearDatabase();
    }

    public async Task DisposeAsync()
    {
        await ClearDatabase();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsToken()
    {
        var command = TestData.Auth.ValidRegisterCommand;

        var response = await _client.PostAsJsonAsync("/api/auth/register", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithExistingUser_ReturnsToken()
    {
        var user = TestData.Users.ValidUser;
        await CreateTestUser(user);
        var command = TestData.Auth.ValidLoginCommand;

        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData("", "Password123!", "John", "Doe", "L'email est requis")]
    [InlineData("invalid-email", "Password123!", "John", "Doe", "Format d'email invalide")]
    [InlineData("test@example.com", "short", "John", "Doe", "Le mot de passe doit contenir au moins 8 caractères")]
    [InlineData("test@example.com", "password123", "John", "Doe", "Le mot de passe doit contenir au moins une majuscule")]
    [InlineData("test@example.com", "Password123", "John", "Doe", "Le mot de passe doit contenir au moins un caractère spécial")]
    [InlineData("test@example.com", "Password123!", "", "Doe", "Le prénom est requis")]
    [InlineData("test@example.com", "Password123!", "John", "", "Le nom est requis")]
    public async Task Register_WithInvalidData_ReturnsBadRequest(string email, string password, string firstName, string lastName, string expectedError)
    {
        var command = new RegisterCommand
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains(expectedError));
    }
    
    [Fact]
    public async Task Register_WithExistingEmail_ReturnsBadRequest()
    {
        var existingUser = TestData.Users.ValidUser;
        await CreateTestUser(existingUser);

        var command = new RegisterCommand
        {
            Email = existingUser.Email,
            Password = "NewPassword123!",
            FirstName = "New",
            LastName = "User"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("existe déjà"));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var registerRequest = new RegisterCommand
        {
            Email = "login.test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginCommand
        {
            Email = "login.test@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginCommand
        {
            Email = "nonexistent@example.com",
            Password = "WrongPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        var command = new LoginCommand
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("notanemail", "Password123!")]
    [InlineData("test@example.com", "")]
    public async Task Login_WithInvalidData_ReturnsBadRequest(string email, string password)
    {
        var command = new LoginCommand
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var user = TestData.Users.ValidUser;
        await CreateTestUser(user);

        var command = new LoginCommand
        {
            Email = user.Email,
            Password = "WrongPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
        result.Errors.Should().Contain("Identifiants invalides");
    }
    
    [Fact]
    public async Task AuthenticatedEndpoint_WithValidToken_ReturnsSuccess()
    {
        var user = await CreateTestUser(TestData.Users.ValidUser);
        var token = await GetAuthenticationToken(user.Email, "Password123!");
        SetAuthorizationHeader(token);

        var response = await _client.GetAsync("/api/TestAuth/protected");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task AuthenticatedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/TestAuth/protected");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PublicEndpoint_WithoutToken_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/api/TestAuth/public");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}