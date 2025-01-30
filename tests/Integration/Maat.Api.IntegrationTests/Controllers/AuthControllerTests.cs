using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Maat.Api.IntegrationTests;
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

    [Fact]
    public async Task Register_WithValidData_ReturnsToken()
    {
        var request = new RegisterCommand
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_WithInvalidData_ReturnsBadRequest()
    {
        var request = new RegisterCommand
        {
            Email = "invalid-email",
            Password = "short",
            FirstName = "",
            LastName = ""
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
}