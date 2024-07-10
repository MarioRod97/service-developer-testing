using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Catalog;

public static class ApiExtensions
{
    public static IEndpointRouteBuilder MapCatalog(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/catalog/");

        group.MapPost("/{vendor}/{application}", AddItemAsync);

        group.MapGet("/{vendor}/{application}/{version}", GetItemAsync);

        return routes;
    }

    public static async Task<Results<Ok<CatalogItemResponse>, NotFound>> GetItemAsync(string vendor,
        string application, string version, IDocumentSession session,
        INormalizeUrlSegments segmentNormalizer)
    {
        var normalizedApplication = segmentNormalizer.Normalize(application);

        var entity = await session.Query<CatalogItemEntity>()
            .Where(c => c.Vendor == vendor && c.Application == application && c.Version == version)
            .SingleOrDefaultAsync();
        // if the entity is null, return a 404.

        if (entity is null)
        {
            return TypedResults.NotFound();
        }

        var response = new CatalogItemResponse
        {
            Vendor = entity.Vendor,
            Application = entity.Application,
            Version = entity.Version,
            AnnualCostPerSeat = entity.AnnualCostPerSeat
        };

        return TypedResults.Ok(response);
    }

    public static async Task<Created<CatalogItemResponse>> AddItemAsync(
        CreateCatalogItemRequest request,
        string vendor,
        string application,
        IDocumentSession session,
        CancellationToken token)

    {
        var response = new CatalogItemResponse()
        {
            AnnualCostPerSeat = request.AnnualCostPerSeat,
            Application = application,
            Vendor = vendor,
            Version = request.Version,
        };

        // Save it to the database
        var entity = new CatalogItemEntity
        {
            Id = Guid.NewGuid(),
            Vendor = response.Vendor,
            Application = application,
            Version = response.Version,
            AnnualCostPerSeat = response.AnnualCostPerSeat,
            IsCommercial = request.IsCommercial,
        };

        session.Store(entity);
        await session.SaveChangesAsync();

        return TypedResults.Created($"/catalog/{vendor}/{application}/{entity.Version}", response);
    }
}

public record CreateCatalogItemRequest
{
    public string Version { get; set; } = string.Empty;
    public bool IsCommercial { get; set; }
    public decimal AnnualCostPerSeat { get; set; }
}

public record CatalogItemResponse
{
    public string Vendor { get; set; } = string.Empty;
    public string Application { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public decimal? AnnualCostPerSeat { get; set; }
}

/*- Vendor
- Application
- Version
- Commercial or FOSS
    - If it is commercial, the annual projected cost per seat.
    - The Sub of the SoftwareControl person that added it, and the date and time it was added. */