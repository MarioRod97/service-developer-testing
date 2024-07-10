using Catalog.Api.Catalog;
using Catalog.Api.Shared;

namespace Catalog.Tests.UnitTests;
public class NormalizingForTheCatalog
{
    [Fact]
    public void CanNormalizeForCatalog()
    {
        INormalizeUrlSegmentsForTheCatalog sut = new BasicSegmentNormalizer();

        var response = sut.NormalizeForCatalog("Microsoft", "Visual Studio Code", "1.19");

        Assert.Equal("microsoft", response.NormalizedVendor);
        Assert.Equal("visual-studio-code", response.NormalizedApplication);
        Assert.Equal("1-19", response.NormalizedVersion);
    }
}