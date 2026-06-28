using System;
using System.Collections.Generic;

namespace BirthdateConstellaDivination.Fortune
{
    public static class LuckRankClassifier
    {
        public static LuckRank Classify(int score) => score switch
        {
            <= FortuneConstants.RankWorstMax => LuckRank.Worst,
            <= FortuneConstants.RankLowMax => LuckRank.Low,
            <= FortuneConstants.RankMidMax => LuckRank.Mid,
            <= FortuneConstants.RankMidHighMax => LuckRank.MidHigh,
            <= FortuneConstants.RankHighMax => LuckRank.High,
            _ => LuckRank.Highest,
        };

        public static string GetMessage(LuckCategory category, LuckRank rank)
        {
            if (Messages.TryGetValue((category, rank), out var msg))
            {
                return msg;
            }
            throw new InvalidOperationException(
                $"No message registered for ({category}, {rank}). " +
                "Ensure all 6 categories × 6 ranks are mapped in LuckRankClassifier.Messages.");
        }

        public static string GetMessageForScore(LuckCategory category, int score) =>
            GetMessage(category, Classify(score));

        private static readonly IReadOnlyDictionary<(LuckCategory Category, LuckRank Rank), string> Messages =
            new Dictionary<(LuckCategory, LuckRank), string>
            {
                // Life (健康運)
                [(LuckCategory.Life, LuckRank.Worst)] = "危ないかも。体調に気をつけよう。",
                [(LuckCategory.Life, LuckRank.Low)] = "油断は禁物。お気をつけて。",
                [(LuckCategory.Life, LuckRank.Mid)] = "そこそこ。今日一日は何とかなりそう。",
                [(LuckCategory.Life, LuckRank.MidHigh)] = "割といい。快適に過ごせそう。",
                [(LuckCategory.Life, LuckRank.High)] = "かなりいい。すごく快適に過ごせそう。",
                [(LuckCategory.Life, LuckRank.Highest)] = "最高！明日も楽しく過ごせるかも。",

                // Gold (金運)
                [(LuckCategory.Gold, LuckRank.Worst)] = "かなりの損失になりそう。",
                [(LuckCategory.Gold, LuckRank.Low)] = "ちょっとツイてないかも。",
                [(LuckCategory.Gold, LuckRank.Mid)] = "いつもと同じくらいかな。",
                [(LuckCategory.Gold, LuckRank.MidHigh)] = "ちょっとラッキーかも。",
                [(LuckCategory.Gold, LuckRank.High)] = "いいね、今日はツイてる！",
                [(LuckCategory.Gold, LuckRank.Highest)] = "最高！億万長者にだってなれちゃうよ！",

                // Study (勉強運)
                [(LuckCategory.Study, LuckRank.Worst)] = "内容があまり頭に入ってこなさそう。",
                [(LuckCategory.Study, LuckRank.Low)] = "忘れっぽくなるかも。",
                [(LuckCategory.Study, LuckRank.Mid)] = "そこそこ。テストで平均点は取れるかも。",
                [(LuckCategory.Study, LuckRank.MidHigh)] = "平均以上の成績を出せそう。",
                [(LuckCategory.Study, LuckRank.High)] = "知識が大きく広がるかも。",
                [(LuckCategory.Study, LuckRank.Highest)] = "範囲丸暗記間違いなし！",

                // Love (恋愛運)
                [(LuckCategory.Love, LuckRank.Worst)] = "2人の間の雰囲気に気をつけて。",
                [(LuckCategory.Love, LuckRank.Low)] = "うっかり失言してしまうことも？",
                [(LuckCategory.Love, LuckRank.Mid)] = "関係は進展なしかも。",
                [(LuckCategory.Love, LuckRank.MidHigh)] = "いい雰囲気にできるかも。",
                [(LuckCategory.Love, LuckRank.High)] = "思い切ってプロポーズ！",
                [(LuckCategory.Love, LuckRank.Highest)] = "すごいね！もう結婚できるかも！",

                // Work (仕事運)
                [(LuckCategory.Work, LuckRank.Worst)] = "ミスを連発しちゃうかも。",
                [(LuckCategory.Work, LuckRank.Low)] = "重要なことを忘れちゃう！？",
                [(LuckCategory.Work, LuckRank.Mid)] = "いつもどおりの日々をおくれそう。",
                [(LuckCategory.Work, LuckRank.MidHigh)] = "上司に怒られなくなるかも。",
                [(LuckCategory.Work, LuckRank.High)] = "いい仕事ができそう。",
                [(LuckCategory.Work, LuckRank.Highest)] = "いいね！昇進間違いなし！",

                // Pattern (対人運)
                [(LuckCategory.Pattern, LuckRank.Worst)] = "ギクシャクした一日になりそう。",
                [(LuckCategory.Pattern, LuckRank.Low)] = "話題が見つからないかも。",
                [(LuckCategory.Pattern, LuckRank.Mid)] = "友達とは友達のまま。",
                [(LuckCategory.Pattern, LuckRank.MidHigh)] = "相手が自分を見直してくれるかも。",
                [(LuckCategory.Pattern, LuckRank.High)] = "話題をさらって目立ちやすい？",
                [(LuckCategory.Pattern, LuckRank.Highest)] = "すごい！みんなの人気者だね！",
            };
    }
}
