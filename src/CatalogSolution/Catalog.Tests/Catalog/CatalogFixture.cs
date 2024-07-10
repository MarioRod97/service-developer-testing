using Alba;
using Catalog.Api.Catalog;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Catalog.Tests.Catalog;
public class CatalogFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; set; } = null!;

    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16.2-bullseye")
        .WithUsername("user")
        .WithDatabase("catalog")
        .WithPassword("password")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        Host = await AlbaHost.For<global::Program>(config =>
        {
            var connectionString = _postgresContainer.GetConnectionString();
            config.UseSetting("ConnectionStrings:data",
               connectionString);

            config.ConfigureServices(services =>
            {
                services.AddScoped<INormalizeUrlSegments, TestingNormalizer>();
            });
        });
    }
    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}

public class TestingNormalizer : INormalizeUrlSegments
{
    public string Normalize(string segment)
    {
        return segment;
    }
}