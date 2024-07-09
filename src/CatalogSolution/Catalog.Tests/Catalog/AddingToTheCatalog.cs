using Alba;
using Catalog.Api.Catalog;

namespace Catalog.Tests.Catalog;

public class AddingToTheCatalog
{
    [Fact]
    public async Task DoIt()
    {
        using var host = await AlbaHost.For<global::Program>();

        var newCatalogItem = new CreateCatalogItemRequest
        {
            Version = "1.91",
            IsCommercial = false,
            AnnualCostPerSeat = 2.99M
        };

        var expectedResponse = new CatalogItemResponse
        {
            Vendor = "Microsoft",
            Application = "VSCode",
            AnnualCostPerSeat = 2.99M,
            Version = "1.91"
        };

        var response = await host.Scenario(api =>
        {
            api.Post.Json(newCatalogItem).ToUrl("/catalog/microsoft/vscode");
            api.StatusCodeShouldBe(201);
        });

        var body = await response.ReadAsJsonAsync<CatalogItemResponse>();

        Assert.NotNull(body);
        Assert.Equal(expectedResponse, body);
    }
}