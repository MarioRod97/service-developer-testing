
using Catalog.Api.Catalog;

namespace Catalog.Tests.UnitTests;
public class NormalizingUrlSegments
{
    [Theory]
    [InlineData("Microsoft", "microsoft")]
    [InlineData("VisualStudio", "visualstudio")]
    [InlineData("Visual Studio", "visual-studio")]
    public void CanNormalizeThese(string example, string expected)
    {
        INormalizeUrlSegments sut = new BasicSegmentNormalizer();

        var result = sut.Normalize(example);

        Assert.Equal(expected, result);
    }
}