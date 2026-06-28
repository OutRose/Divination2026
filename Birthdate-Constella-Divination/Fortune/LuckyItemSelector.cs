using System;

namespace BirthdateConstellaDivination.Fortune
{
    public static class LuckyItemSelector
    {
        public static LuckyItem Select(int value) => value switch
        {
            1 => LuckyItem.Pearl,
            >= 2 and <= 4 => LuckyItem.Globe,
            >= 5 and <= 7 => LuckyItem.Charm,
            >= 8 and <= 10 => LuckyItem.LeisureSheet,
            _ => throw new ArgumentOutOfRangeException(
                nameof(value), value,
                "Value must be between 1 and 10 (inclusive)."),
        };

        public static LuckyItem Select(Random rng)
        {
            if (rng is null)
            {
                throw new ArgumentNullException(nameof(rng));
            }
            return Select(rng.Next(
                FortuneConstants.LuckyItemMinInclusive,
                FortuneConstants.LuckyItemMaxExclusive));
        }

        public static string GetName(LuckyItem item) => item switch
        {
            LuckyItem.Pearl => "真珠",
            LuckyItem.Globe => "地球儀",
            LuckyItem.Charm => "御守り",
            LuckyItem.LeisureSheet => "レジャーシート",
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null),
        };
    }
}
