using System;
using BirthdateConstellaDivination.Fortune;
using Xunit;

namespace BirthdateConstellaDivination.Tests.Fortune;

public sealed class FortuneCalculatorTests
{
    [Fact]
    public void Calculate_BirthdateEqualsToday_AllScoresAreZeroAdjusted()
    {
        var today = new DateTime(2026, 6, 28);
        var birth = new DateTime(2026, 6, 28);

        // 誕生日 == 今日 → calresult = 0 → 全 digit が 0 → 全スコアが 0 → すべてゼロ補正
        // (rng は使われるが、digits が全部 0 なので rdkey/lfadj の値に関係なくスコアは 0)
        var fortune = FortuneCalculator.Calculate(birth, today, new Random(42));

        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroLifeSeed),    fortune.Life);
        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroGoldSeed),    fortune.Gold);
        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroStudySeed),   fortune.Study);
        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroLoveSeed),    fortune.Love);
        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroWorkSeed),    fortune.Work);
        Assert.Equal(ExpectedZeroAdjusted(FortuneConstants.ZeroPatternSeed), fortune.Pattern);
    }

    [Fact]
    public void Calculate_KnownInputs_ProducesExpectedScores()
    {
        // birth=2002-05-23, today=2026-06-28 → calresult = 20260628 - 20020523 = 240105
        // digits (LSB first) = [5, 0, 1, 0, 4, 2]
        // digits[5]=2 (life)、digits[4]=4 (gold)、digits[3]=0 (study)、digits[2]=1 (love)、digits[1]=0 (work)、digits[0]=5 (pattern)
        var today = new DateTime(2026, 6, 28);
        var birth = new DateTime(2002, 5, 23);

        // 期待値計算用に同じ seed の rng で rdkey/lfadj を取り出す
        var rng = new Random(42);
        int rdkey = rng.Next(FortuneConstants.RandomKeyMinInclusive,       FortuneConstants.RandomKeyMaxExclusive);
        int lfadj = rng.Next(FortuneConstants.LifeAdjustmentMinInclusive, FortuneConstants.LifeAdjustmentMaxExclusive);

        int expectedLife    = 2 * rdkey * lfadj;
        int expectedGold    = 4 * rdkey;
        int expectedStudyRaw = 0 * rdkey;  // → zero-adjusted
        int expectedLove    = 1 * rdkey;
        int expectedWorkRaw = 0 * rdkey;   // → zero-adjusted
        int expectedPattern = 5 * rdkey;
        if (expectedPattern > FortuneConstants.MaxScore)
        {
            expectedPattern /= lfadj;
        }

        int expectedStudy = expectedStudyRaw == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroStudySeed) : expectedStudyRaw;
        int expectedWork  = expectedWorkRaw  == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroWorkSeed)  : expectedWorkRaw;
        int expectedLifeFinal = expectedLife == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroLifeSeed)  : expectedLife;
        int expectedGoldFinal = expectedGold == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroGoldSeed)  : expectedGold;
        int expectedLoveFinal = expectedLove == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroLoveSeed)  : expectedLove;
        int expectedPatternFinal = expectedPattern == 0 ? ExpectedZeroAdjusted(FortuneConstants.ZeroPatternSeed) : expectedPattern;

        // Calculate を実 rng (seed=42 で再生成) で呼び、上で計算した期待値と一致するか
        var fortune = FortuneCalculator.Calculate(birth, today, new Random(42));

        Assert.Equal(expectedLifeFinal,    fortune.Life);
        Assert.Equal(expectedGoldFinal,    fortune.Gold);
        Assert.Equal(expectedStudy,        fortune.Study);
        Assert.Equal(expectedLoveFinal,    fortune.Love);
        Assert.Equal(expectedWork,         fortune.Work);
        Assert.Equal(expectedPatternFinal, fortune.Pattern);
    }

    [Fact]
    public void Calculate_NullRng_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FortuneCalculator.Calculate(new DateTime(2002, 5, 23), new DateTime(2026, 6, 28), null!));
    }

    [Theory]
    [InlineData(2002, 5, 23, 2026, 6, 28)]
    [InlineData(1999, 1, 1, 2026, 6, 28)]
    [InlineData(2026, 1, 1, 2026, 6, 28)]
    public void Calculate_AllScoresAreStrictlyPositive(int by, int bm, int bd, int ty, int tm, int td)
    {
        // 元実装の不変量: ゼロ補正によりすべての最終スコアは >= 1。
        // 注: Life スコアの上限は MaxScore (27) を超えうる (例: 9×3×6=162)。
        // これは元実装の挙動 (Designer 側の ProgressBar.Maximum=27 と矛盾) を保存している。
        // 将来 γ または δ で Life キャップの是非を判断する。
        var birth = new DateTime(by, bm, bd);
        var today = new DateTime(ty, tm, td);
        var fortune = FortuneCalculator.Calculate(birth, today, new Random(42));

        Assert.True(fortune.Life >= FortuneConstants.MinScore);
        Assert.True(fortune.Gold >= FortuneConstants.MinScore);
        Assert.True(fortune.Study >= FortuneConstants.MinScore);
        Assert.True(fortune.Love >= FortuneConstants.MinScore);
        Assert.True(fortune.Work >= FortuneConstants.MinScore);
        Assert.True(fortune.Pattern >= FortuneConstants.MinScore);
    }

    [Fact]
    public void Calculate_DeterministicWithSameSeed()
    {
        var today = new DateTime(2026, 6, 28);
        var birth = new DateTime(2002, 5, 23);

        var a = FortuneCalculator.Calculate(birth, today, new Random(42));
        var b = FortuneCalculator.Calculate(birth, today, new Random(42));

        Assert.Equal(a, b);
    }

    private static int ExpectedZeroAdjusted(int seed) =>
        new Random(seed).Next(
            FortuneConstants.ZeroAdjustmentMinInclusive,
            FortuneConstants.ZeroAdjustmentMaxExclusive);
}
