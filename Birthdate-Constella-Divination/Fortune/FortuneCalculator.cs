using System;

namespace BirthdateConstellaDivination.Fortune
{
    public static class FortuneCalculator
    {
        public static Fortune Calculate(int birthdateYyyyMmDd, DateTime today, Random rng)
        {
            if (rng is null)
            {
                throw new ArgumentNullException(nameof(rng));
            }

            int todayYyyyMmDd = int.Parse(today.ToString("yyyyMMdd"));
            int calresult = todayYyyyMmDd - birthdateYyyyMmDd;

            int[] digits = new int[FortuneConstants.DigitCount];
            for (int i = 0; i < FortuneConstants.DigitCount; i++)
            {
                digits[i] = calresult % FortuneConstants.DigitBase;
                if (i < FortuneConstants.DigitCount - 1)
                {
                    calresult /= FortuneConstants.DigitBase;
                }
            }

            int rdkey = rng.Next(FortuneConstants.RandomKeyMinInclusive, FortuneConstants.RandomKeyMaxExclusive);
            int lfadj = rng.Next(FortuneConstants.LifeAdjustmentMinInclusive, FortuneConstants.LifeAdjustmentMaxExclusive);

            int life = digits[5] * rdkey * lfadj;
            int gold = digits[4] * rdkey;
            int study = digits[3] * rdkey;
            int love = digits[2] * rdkey;
            int work = digits[1] * rdkey;
            int pattern = digits[0] * rdkey;

            // 元実装の挙動を保存: > 27 キャップは pattern に適用される。
            // pattern の理論最大は 9*3=27 なので分岐は実質到達不能 (dead branch)。
            // 元コードの意図は life の > 27 キャップだった可能性が高いが、ここでは挙動を変えない。
            if (pattern > FortuneConstants.MaxScore)
            {
                pattern /= lfadj;
            }

            return new Fortune(
                Life: ZeroAdjust(life, FortuneConstants.ZeroLifeSeed),
                Gold: ZeroAdjust(gold, FortuneConstants.ZeroGoldSeed),
                Study: ZeroAdjust(study, FortuneConstants.ZeroStudySeed),
                Love: ZeroAdjust(love, FortuneConstants.ZeroLoveSeed),
                Work: ZeroAdjust(work, FortuneConstants.ZeroWorkSeed),
                Pattern: ZeroAdjust(pattern, FortuneConstants.ZeroPatternSeed)
            );
        }

        private static int ZeroAdjust(int score, int seed)
        {
            if (score != 0)
            {
                return score;
            }

            return new Random(seed).Next(
                FortuneConstants.ZeroAdjustmentMinInclusive,
                FortuneConstants.ZeroAdjustmentMaxExclusive);
        }
    }
}
