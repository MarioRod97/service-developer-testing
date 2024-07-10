namespace Catalog.Api.Catalog;

public interface INormalizeUrlSegmentsForTheCatalog
{
    public NormalizationResults NormalizeForCatalog(string vendor, string application, string version);
}

public record NormalizationResults(
    string NormalizedVendor, string NormalizedApplication,
    string NormalizedVersion);