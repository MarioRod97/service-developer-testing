using Catalog.Api.Catalog;
using Slugify;

namespace Catalog.Api.Shared;

public class BasicSegmentNormalizer : INormalizeUrlSegmentsForTheCatalog
{
    private readonly SlugHelper slugHelper;
    public BasicSegmentNormalizer()
    {
        var config = new SlugHelperConfiguration();
        config.StringReplacements.Add(".", "-");
        slugHelper = new SlugHelper(config);

    }
    public string Normalize(string segment) =>
        slugHelper.GenerateSlug(segment);

    public NormalizationResults NormalizeForCatalog(string vendor, string application, string version)
    {
        return new NormalizationResults(Normalize(vendor),
            Normalize(application), Normalize(version));
    }
}