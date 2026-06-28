using Xunit;

namespace BirthdateConstellaDivination.Tests;

public sealed class SanityTests
{
    [Fact]
    public void Framework_Works()
    {
        Assert.True(true);
    }

    [Fact]
    public void MainAssembly_IsReachable()
    {
        var firstWindowType = typeof(FirstWindow);
        Assert.NotNull(firstWindowType);
        Assert.Equal("BirthdateConstellaDivination", firstWindowType.Namespace);
    }
}
