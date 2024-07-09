namespace Catalog.Api.Catalog;

public class CatalogItemEntity
{
    public Guid Id { get; set; }
    public string Vendor { get; set; } = string.Empty;
    public string Application { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public decimal? AnnualCostPerSeat { get; set; }
    public bool IsCommercial { get; set; }
}
