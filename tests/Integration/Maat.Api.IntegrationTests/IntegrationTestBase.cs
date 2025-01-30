using System.Net.Http.Headers;
using System.Net.Http.Json;
using Maat.Api.IntegrationTests.Fixtures;
using Maat.Application.Features.Auth.Commands.Login;
using Maat.Domain.Entities;
using Maat.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Maat.Service;
using Maat.Shared.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace Maat.Api.IntegrationTests;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected readonly IServiceScope _scope;
    protected readonly MaatDbContext _dbContext;

    protected IntegrationTestBase(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<MaatDbContext>();
    }
    
    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MaatDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<MaatDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
                });
            });
        });

        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<MaatDbContext>();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await ClearDatabase();
    }

    public void Dispose()
    {
        _scope.Dispose();
        _client.Dispose();
    }

    protected async Task ClearDatabase()
    {
        _dbContext.Users.RemoveRange(_dbContext.Users);
        _dbContext.Companies.RemoveRange(_dbContext.Companies);
        _dbContext.Diagnostics.RemoveRange(_dbContext.Diagnostics);
        
        await _dbContext.SaveChangesAsync();
    }
    
    protected async Task<User> CreateTestUser(User user)
    {
        var addedUser = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return addedUser.Entity;
    }

    protected async Task<Company> CreateTestCompany(Company company)
    {
        var addedCompany = await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();
        return addedCompany.Entity;
    }

    protected async Task<string> GetAuthenticationToken(string email, string password)
    {
        var loginCommand = new LoginCommand { Email = email, Password = password };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginCommand);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return result?.Token ?? string.Empty;
    }

    protected void SetAuthorizationHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
}