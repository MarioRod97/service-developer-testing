using Catalog.Api.Shared;

namespace Catalog.Tests.UnitTests;
public class BasicSegmentNormalizerTests
{
    [Theory]
    [InlineData("Microsoft", "microsoft")]
    [InlineData("VisualStudio", "visualstudio")]
    [InlineData("Visual Studio", "visual-studio")]
    [InlineData("docker inc. docker 🐳 desktop", "docker-inc-docker-desktop")]
    public void CanNormalizeThese(string example, string expected)
    {
        var sut = new BasicSegmentNormalizer();

        var result = sut.Normalize(example);

        Assert.Equal(expected, result);

    }
}