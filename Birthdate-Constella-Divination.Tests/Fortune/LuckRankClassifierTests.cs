using System;
using BirthdateConstellaDivination.Fortune;
using Xunit;

namespace BirthdateConstellaDivination.Tests.Fortune;

public sealed class LuckRankClassifierTests
{
    [Theory]
    // 境界値: 各レンジの両端 + 範囲外
    [InlineData(-5, LuckRank.Worst)]   // 負数 → Worst (キャップ)
    [InlineData(0, LuckRank.Worst)]
    [InlineData(4, LuckRank.Worst)]
    [InlineData(5, LuckRank.Low)]
    [InlineData(9, LuckRank.Low)]
    [InlineData(10, LuckRank.Mid)]
    [InlineData(14, LuckRank.Mid)]
    [InlineData(15, LuckRank.MidHigh)]
    [InlineData(19, LuckRank.MidHigh)]
    [InlineData(20, LuckRank.High)]
    [InlineData(24, LuckRank.High)]
    [InlineData(25, LuckRank.Highest)]
    [InlineData(27, LuckRank.Highest)]
    [InlineData(28, LuckRank.Highest)]  // 範囲外上限 → Highest (キャップ)
    [InlineData(100, LuckRank.Highest)] // 大きな値も Highest
    public void Classify_MapsScoreToCorrectRank(int score, LuckRank expected)
    {
        Assert.Equal(expected, LuckRankClassifier.Classify(score));
    }

    [Theory]
    [InlineData(LuckCategory.Life, LuckRank.Worst, "危ないかも。体調に気をつけよう。")]
    [InlineData(LuckCategory.Life, LuckRank.Highest, "最高！明日も楽しく過ごせるかも。")]
    [InlineData(LuckCategory.Gold, LuckRank.Worst, "かなりの損失になりそう。")]
    [InlineData(LuckCategory.Gold, LuckRank.Highest, "最高！億万長者にだってなれちゃうよ！")]
    [InlineData(LuckCategory.Study, LuckRank.Mid, "そこそこ。テストで平均点は取れるかも。")]
    [InlineData(LuckCategory.Love, LuckRank.High, "思い切ってプロポーズ！")]
    [InlineData(LuckCategory.Work, LuckRank.MidHigh, "上司に怒られなくなるかも。")]
    [InlineData(LuckCategory.Pattern, LuckRank.Low, "話題が見つからないかも。")]
    [InlineData(LuckCategory.Pattern, LuckRank.Highest, "すごい！みんなの人気者だね！")]
    public void GetMessage_ReturnsExactStringFromOriginalImplementation(LuckCategory category, LuckRank rank, string expected)
    {
        Assert.Equal(expected, LuckRankClassifier.GetMessage(category, rank));
    }

    [Fact]
    public void GetMessage_AllSixCategoriesTimesSixRanksAreRegistered()
    {
        // 6 × 6 = 36 すべての組合せに対してメッセージが登録されていることを確認
        foreach (LuckCategory category in Enum.GetValues(typeof(LuckCategory)))
        {
            foreach (LuckRank rank in Enum.GetValues(typeof(LuckRank)))
            {
                var msg = LuckRankClassifier.GetMessage(category, rank);
                Assert.False(string.IsNullOrWhiteSpace(msg),
                    $"Message for ({category}, {rank}) must not be empty or whitespace");
            }
        }
    }

    [Theory]
    // GetMessageForScore は Classify + GetMessage の合成。代表ケースで検証。
    [InlineData(LuckCategory.Life, 0, "危ないかも。体調に気をつけよう。")]
    [InlineData(LuckCategory.Life, 25, "最高！明日も楽しく過ごせるかも。")]
    [InlineData(LuckCategory.Gold, 12, "いつもと同じくらいかな。")]
    [InlineData(LuckCategory.Pattern, 27, "すごい！みんなの人気者だね！")]
    public void GetMessageForScore_CombinesClassifyAndGetMessage(LuckCategory category, int score, string expected)
    {
        Assert.Equal(expected, LuckRankClassifier.GetMessageForScore(category, score));
    }
}
