using BirthdateConstellaDivination.Fortune;
using Xunit;

namespace BirthdateConstellaDivination.Tests.Fortune;

public sealed class FortuneTests
{
    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0, false)]
    [InlineData(27, 0, 0, 0, 0, 0, 1, false)]
    [InlineData(27, 27, 0, 0, 0, 0, 2, false)]
    [InlineData(27, 27, 27, 0, 0, 0, 3, true)]
    [InlineData(0, 27, 0, 27, 0, 27, 3, true)]
    [InlineData(27, 27, 27, 27, 0, 0, 4, true)]
    [InlineData(27, 27, 27, 27, 27, 27, 6, true)]
    [InlineData(26, 27, 27, 27, 27, 27, 5, true)]
    [InlineData(1, 5, 10, 15, 20, 24, 0, false)]
    public void MaxScoreCount_And_IsSuperLucky_Reflect_Number_Of_27s(
        int life, int gold, int study, int love, int work, int pattern,
        int expectedMaxCount, bool expectedSuperLucky)
    {
        var fortune = new global::BirthdateConstellaDivination.Fortune.Fortune(
            life, gold, study, love, work, pattern);

        Assert.Equal(expectedMaxCount, fortune.MaxScoreCount);
        Assert.Equal(expectedSuperLucky, fortune.IsSuperLucky);
    }

    [Fact]
    public void Record_Equality_Works_For_Identical_Scores()
    {
        var a = new global::BirthdateConstellaDivination.Fortune.Fortune(1, 2, 3, 4, 5, 6);
        var b = new global::BirthdateConstellaDivination.Fortune.Fortune(1, 2, 3, 4, 5, 6);
        var c = new global::BirthdateConstellaDivination.Fortune.Fortune(1, 2, 3, 4, 5, 7);

        Assert.Equal(a, b);
        Assert.NotEqual(a, c);
    }
}
