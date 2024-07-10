using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Catalog;

public static class ApiExtensions
{
    public static IEndpointRouteBuilder MapCatalog(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/catalog/");
        group.MapPost("/{vendor}/{application}", AddItemAsync);
        group.MapGet("/{vendor:regex(^[a-zA-Z]+$)}/{application}/{version}", GetItemAsync);

        return routes;
    }

    public static async Task<Results<Ok<CatalogItemResponse>, NotFound>> GetItemAsync(string vendor,
        string application, string version, IDocumentSession session,
        INormalizeUrlSegmentsForTheCatalog slugger)
    {
        var slugs = slugger.NormalizeForCatalog(vendor, application, version);

        // Write the Code You Wish You Had
        var locationSlug = slugs.GetLocationSlug();

        // this will get better in a second
        //var entity = await session.Query<CatalogItemEntity>()
        //    .Where(c => c.Vendor == slugs.NormalizedVendor
        //    && c.Application == slugs.NormalizedApplication &&
        //    c.Version == slugs.NormalizedVersion)
        //    .SingleOrDefaultAsync();
        var entity = await session.Query<CatalogItemEntity>()
            .Where(c => c.Slug == locationSlug).SingleOrDefaultAsync();
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

    public static async Task<Results<Created<CatalogItemResponse>,
        BadRequest<IDictionary<string, string[]>>>>
        AddItemAsync(
        CreateCatalogItemRequest request,
        string vendor,
        string application,
        INormalizeUrlSegmentsForTheCatalog slugger,
        IDocumentSession session,
        CancellationToken token,
        IValidator<CreateCatalogItemRequest> validator)

    {
        //TODO: 2.Validating
        //TODO: 3. How To Test This?
        var validations = await validator.ValidateAsync(request);

        if (!validations.IsValid)
        {
            return TypedResults.BadRequest(validations.ToDictionary());
        }

        var slugs = slugger.NormalizeForCatalog(vendor, application, request.Version);

        var response = new CatalogItemResponse()
        {
            AnnualCostPerSeat = request.AnnualCostPerSeat,
            Application = slugs.NormalizedApplication,
            Vendor = slugs.NormalizedVendor,
            Version = slugs.NormalizedVersion

        };

        // If it isn't free, then check the budget to see if we have enough money,
        // if we don't, return a 400 with an explanation.
        if (request.IsCommercial && request.AnnualCostPerSeat > 0)
        {
            // Call another API, and ask if we have enough money.
            // If it says yes, cool,
            // if not, return a 400 saying we can't afford this.
        }

        // Save it to the database
        var locationSlug = slugs.GetLocationSlug();
        var entity = new CatalogItemEntity
        {
            Id = Guid.NewGuid(),
            Vendor = response.Vendor,
            Application = application,
            Version = response.Version,
            AnnualCostPerSeat = response.AnnualCostPerSeat,
            IsCommercial = request.IsCommercial,
            Slug = locationSlug
        };

        session.Store(entity);
        await session.SaveChangesAsync();

        return TypedResults.Created(locationSlug, response);
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