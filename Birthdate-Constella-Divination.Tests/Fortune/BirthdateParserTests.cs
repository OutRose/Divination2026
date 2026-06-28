using System;
using BirthdateConstellaDivination.Fortune;
using Xunit;

namespace BirthdateConstellaDivination.Tests.Fortune;

public sealed class BirthdateParserTests
{
    [Theory]
    [InlineData("20020523", 2002, 5, 23)]
    [InlineData("19990101", 1999, 1, 1)]
    [InlineData("20260628", 2026, 6, 28)]
    [InlineData("00010101", 1, 1, 1)]      // DateTime.MinValue 相当 (年=1)
    [InlineData("99991231", 9999, 12, 31)] // DateTime.MaxValue 相当
    public void TryParse_ValidInput_ReturnsTrue(string text, int y, int m, int d)
    {
        bool ok = BirthdateParser.TryParse(text, out var date, out var error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.Equal(new DateTime(y, m, d), date);
    }

    [Fact]
    public void TryParse_Null_ReturnsEmptyError()
    {
        bool ok = BirthdateParser.TryParse(null, out _, out var error);

        Assert.False(ok);
        Assert.Equal(BirthdateError.Empty, error);
    }

    [Fact]
    public void TryParse_Empty_ReturnsEmptyError()
    {
        bool ok = BirthdateParser.TryParse("", out _, out var error);

        Assert.False(ok);
        Assert.Equal(BirthdateError.Empty, error);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("123")]
    [InlineData("1234567")]   // 7 桁
    [InlineData("123456789")] // 9 桁
    [InlineData(" 12345678")] // 先頭空白入りで 9 桁
    [InlineData("12345678 ")] // 末尾空白入りで 9 桁
    public void TryParse_WrongLength_ReturnsNotEightDigitsError(string text)
    {
        bool ok = BirthdateParser.TryParse(text, out _, out var error);

        Assert.False(ok);
        Assert.Equal(BirthdateError.NotEightDigits, error);
    }

    [Theory]
    [InlineData("abcdefgh")]
    [InlineData("2002.523")] // 8 文字だが '.' 含む
    [InlineData("2002a523")] // 8 文字だが 'a' 含む
    [InlineData("-2002523")] // 8 文字だが '-' 含む
    [InlineData("2002 523")] // 8 文字だが空白含む
    [InlineData("２００２０５２３")] // 全角数字 (8 文字だが ASCII 数字ではない)
    public void TryParse_NonNumeric_ReturnsNotNumericError(string text)
    {
        bool ok = BirthdateParser.TryParse(text, out _, out var error);

        Assert.False(ok);
        Assert.Equal(BirthdateError.NotNumeric, error);
    }

    [Theory]
    [InlineData("20020230")] // Feb 30 (存在しない日)
    [InlineData("20021301")] // 月 13 (存在しない月)
    [InlineData("20020000")] // 月 0、日 0
    [InlineData("00000000")] // 全ゼロ (年 0)
    [InlineData("00001231")] // 年 0 だが月日は有効
    [InlineData("20020431")] // 4 月 31 日 (存在しない)
    public void TryParse_InvalidDate_ReturnsInvalidDateError(string text)
    {
        bool ok = BirthdateParser.TryParse(text, out _, out var error);

        Assert.False(ok);
        Assert.Equal(BirthdateError.InvalidDate, error);
    }

    [Theory]
    [InlineData(BirthdateError.Empty)]
    [InlineData(BirthdateError.NotEightDigits)]
    [InlineData(BirthdateError.NotNumeric)]
    [InlineData(BirthdateError.InvalidDate)]
    public void DescribeError_AllEnumValues_ReturnsNonEmptyMessage(BirthdateError error)
    {
        string message = BirthdateParser.DescribeError(error);

        Assert.False(string.IsNullOrEmpty(message));
    }

    [Fact]
    public void DescribeError_UndefinedValue_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BirthdateParser.DescribeError((BirthdateError)999));
    }
}
