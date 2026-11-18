using ForeignCard.Common.Constants;
using ForeignCard.Common.Helpers;

namespace ForeignCard.Tests.Helpers;

public class CurrencyCodeTests
{
    [Theory]
    [InlineData("840", "USD")]
    [InlineData("978", "EUR")]
    [InlineData("826", "GBP")]
    public void ToAlphaCode_BilinenKod_HarfKoduDoner(string numeric, string expected)
    {
        Assert.Equal(expected, CurrencyCode.ToAlphaCode(numeric));
    }

    [Fact]
    public void ToAlphaCode_BilinmeyenKod_NullDoner()
    {
        Assert.Null(CurrencyCode.ToAlphaCode("999"));
    }

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("XYZ", false)]
    public void IsSupported_DogruSonucVerir(string alpha, bool expected)
    {
        Assert.Equal(expected, CurrencyCode.IsSupported(alpha));
    }
}
