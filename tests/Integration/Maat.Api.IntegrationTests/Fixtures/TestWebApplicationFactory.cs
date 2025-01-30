using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Maat.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Maat.Service;

namespace Maat.Api.IntegrationTests.Fixtures;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer;

    public TestWebApplicationFactory()
    {
        _dbContainer = new PostgreSqlTestcontainer();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
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
                options.UseNpgsql(_dbContainer.ConnectionString));
        });
    }

    public async Task InitializeAsync() => await _dbContainer.InitializeAsync();
    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}