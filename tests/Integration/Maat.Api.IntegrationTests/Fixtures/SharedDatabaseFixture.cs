using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Maat.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Maat.Service;

namespace Maat.Api.IntegrationTests.Fixtures;

public class SharedDatabaseFixture : WebApplicationFactory<Program>
{
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
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}