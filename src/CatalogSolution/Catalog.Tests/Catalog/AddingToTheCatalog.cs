using Alba;
using Catalog.Api.Catalog;


namespace Catalog.Tests.Catalog;
public class AddingToTheCatalog : IClassFixture<CatalogFixture>
{

    private IAlbaHost _host;
    public AddingToTheCatalog(CatalogFixture fixture)
    {
        _host = fixture.Host;
    }


    [Fact]
    public async Task DoIt()
    {
        var newCatalogItem = new CreateCatalogItemRequest
        {
            Version = "1.91",
            IsCommercial = true,
            AnnualCostPerSeat = 2.99M
        };

        var expectedResponse = new CatalogItemResponse
        {
            Vendor = "microsoft",
            Application = "visualstudio",
            AnnualCostPerSeat = 2.99M,
            Version = "1.91"
        };

        var postResponse = await _host.Scenario(api =>
        {
            api.Post.Json(newCatalogItem).ToUrl("/catalog/microsoft/visualstudio");
            api.StatusCodeShouldBe(201);
        });

        var locationHeader = postResponse.Context.Response.Headers["location"].Single();

        Assert.NotNull(locationHeader);

        var postBody = await postResponse.ReadAsJsonAsync<CatalogItemResponse>();

        Assert.NotNull(postBody);
        Assert.Equal(expectedResponse, postBody);

        var getResponse = await _host.Scenario(api =>
        {
            api.Get.Url(locationHeader);
            api.StatusCodeShouldBeOk();
        });

        var getBody = await getResponse.ReadAsJsonAsync<CatalogItemResponse>();

        Assert.Equal(postBody, getBody);
    }

    [Fact]
    public async Task GettingAnItemThatIsntInTheCatalog()
    {
        await _host.Scenario(api =>
        {
            api.Get.Url("/catalog/microsoft/vscode/1.17");
            api.StatusCodeShouldBe(404);
        });
    }
}