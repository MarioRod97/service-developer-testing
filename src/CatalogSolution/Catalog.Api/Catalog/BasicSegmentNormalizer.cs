namespace Catalog.Api.Catalog;

public class BasicSegmentNormalizer : INormalizeUrlSegments
{
    public string Normalize(string segment)
    {
        return segment.ToLower();
    }
}
