using Alba;
using Catalog.Api.Catalog;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
            config.ConfigureServices((sp) =>
            {
                ConfigureMyServices(sp);

            });
        });
    }

    protected virtual void ConfigureMyServices(IServiceCollection services)
    {
        // Template method
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}

public class TestingCatalogFixture : CatalogFixture
{
    protected override void ConfigureMyServices(IServiceCollection services)
    {
        var fakeSlugThing = Substitute.For<INormalizeUrlSegmentsForTheCatalog>();
        fakeSlugThing.NormalizeForCatalog(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Throws(new Exception("Blammo"));

        services.AddScoped<INormalizeUrlSegmentsForTheCatalog>(_ => fakeSlugThing);
    }
}