namespace Catalog.Tests.Catalog;

public class NegativeCatalogTests(TestingCatalogFixture fixture)
    : IClassFixture<TestingCatalogFixture>
{
    [Fact]
    public async Task GetAFourOFourForBadUrl()
    {
        var invalidUrlBecauseOfSpaces = "/catalog/Jet Brains/Rider/17";
        await fixture.Host.Scenario(api =>
        {
            api.Get.Url(invalidUrlBecauseOfSpaces);
            api.StatusCodeShouldBe(404);
        });
    }
}