using Testcontainers.PostgreSql;

namespace Maat.Api.IntegrationTests.Fixtures;

public class PostgreSqlTestcontainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    
    public string ConnectionString => _container.GetConnectionString();

    public PostgreSqlTestcontainer()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("maat_test_db")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .WithCleanUp(true)
            .Build();
    }

    public Task InitializeAsync() => _container.StartAsync();
    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}