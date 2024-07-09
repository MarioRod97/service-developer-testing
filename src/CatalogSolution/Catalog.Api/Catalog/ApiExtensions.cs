using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Catalog;

public static class ApiExtensions
{
    public static IEndpointRouteBuilder MapCatalog(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/catalg");

        group.MapPost("/{vendor}/{application}", AddItemAsync);

        return routes;
    }

    public static async Task<Created<CatalogItemResponse>> AddItemAsync(CreateCatalogItemRequest request,
        string vendor,
        string application,
        CancellationToken token)
    {
        var response = new CatalogItemResponse()
        {
            AnnualCostPerSeat = request.AnnualCostPerSeat,
            Application = application,
            Vendor = vendor,
            Version = request.Version
        };

        return TypedResults.Created("slime", response);
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