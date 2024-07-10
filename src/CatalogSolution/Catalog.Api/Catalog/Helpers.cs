namespace Catalog.Api.Catalog;

public static class Helpers
{
    public static string GetLocationSlug(this NormalizationResults results)
    {
        return $"/catalog/{results.NormalizedVendor}/{results.NormalizedApplication}/{results.NormalizedVersion}";
    }
}