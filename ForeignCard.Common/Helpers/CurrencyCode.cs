using ForeignCard.Common.Constants;

namespace ForeignCard.Common.Helpers;


public static class CurrencyCode
{
    public static IReadOnlyCollection<string>AllNumericCodes
        => CurrencyConstants.ExchangeRates.Select(x => x.Numeric).ToList();
    public static bool IsSupported(string alphaCode) 
        => CurrencyConstants.ExchangeRates.Any(x => x.Alpha == alphaCode);
    public static string? ToAlphaCode(string numericCode)
        => CurrencyConstants.ExchangeRates.FirstOrDefault(x => x.Numeric == numericCode).Alpha;
}