using System;
using BirthdateConstellaDivination.Fortune;
using Xunit;

namespace BirthdateConstellaDivination.Tests.Fortune;

public sealed class LuckyItemSelectorTests
{
    [Theory]
    [InlineData(1, LuckyItem.Pearl)]
    [InlineData(2, LuckyItem.Globe)]
    [InlineData(3, LuckyItem.Globe)]
    [InlineData(4, LuckyItem.Globe)]
    [InlineData(5, LuckyItem.Charm)]
    [InlineData(6, LuckyItem.Charm)]
    [InlineData(7, LuckyItem.Charm)]
    [InlineData(8, LuckyItem.LeisureSheet)]
    [InlineData(9, LuckyItem.LeisureSheet)]
    [InlineData(10, LuckyItem.LeisureSheet)]
    public void Select_FromInt_MapsValueToItem(int value, LuckyItem expected)
    {
        Assert.Equal(expected, LuckyItemSelector.Select(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(100)]
    public void Select_FromInt_OutOfRange_Throws(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LuckyItemSelector.Select(value));
    }

    [Fact]
    public void Select_FromRandom_IsDeterministicWithSameSeed()
    {
        var a = LuckyItemSelector.Select(new Random(42));
        var b = LuckyItemSelector.Select(new Random(42));
        Assert.Equal(a, b);
    }

    [Fact]
    public void Select_FromRandom_AlwaysReturnsValidItem()
    {
        // Random.Next(1, 10) は 1〜9 を返すので、LeisureSheet も 8/9 経由で出る。
        // 1000 回回して常に Pearl/Globe/Charm/LeisureSheet のいずれかであることを確認。
        var rng = new Random(123);
        for (int i = 0; i < 1000; i++)
        {
            var item = LuckyItemSelector.Select(rng);
            Assert.True(
                item == LuckyItem.Pearl ||
                item == LuckyItem.Globe ||
                item == LuckyItem.Charm ||
                item == LuckyItem.LeisureSheet,
                $"Unexpected enum value: {item}");
        }
    }

    [Fact]
    public void Select_FromRandom_NullThrows()
    {
        Assert.Throws<ArgumentNullException>(() => LuckyItemSelector.Select((Random)null!));
    }

    [Theory]
    [InlineData(LuckyItem.Pearl, "真珠")]
    [InlineData(LuckyItem.Globe, "地球儀")]
    [InlineData(LuckyItem.Charm, "御守り")]
    [InlineData(LuckyItem.LeisureSheet, "レジャーシート")]
    public void GetName_ReturnsJapaneseLabel(LuckyItem item, string expected)
    {
        Assert.Equal(expected, LuckyItemSelector.GetName(item));
    }

    [Fact]
    public void GetName_InvalidEnumValueThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LuckyItemSelector.GetName((LuckyItem)999));
    }
}
