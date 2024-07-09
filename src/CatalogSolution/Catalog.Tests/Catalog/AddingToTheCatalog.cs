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
            IsCommercial = true,
            AnnualCostPerSeat = 2.99M
        };

        var expectedResponse = new CatalogItemResponse
        {
            Vendor = "microsoft",
            Application = "vscode",
            AnnualCostPerSeat = 2.99M,
            Version = "1.91"
        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(newCatalogItem).ToUrl("/catalog/microsoft/vscode");
            api.StatusCodeShouldBe(201);
        });

        var postBody = await postResponse.ReadAsJsonAsync<CatalogItemResponse>();

        Assert.NotNull(postBody);
        Assert.Equal(expectedResponse, postBody);

        var getResponse = await host.Scenario(api =>
        {

            api.Get.Url("/catalog/microsoft/vscode/1.91");

            api.StatusCodeShouldBeOk();

        });

        var getBody = await getResponse.ReadAsJsonAsync<CatalogItemResponse>();

        Assert.Equal(postBody, getBody);
    }
}
