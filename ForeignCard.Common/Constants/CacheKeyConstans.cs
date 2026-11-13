namespace ForeignCard.Common.Constants;

// Tüm cache anahtarları tek yerde
public static class CacheKeyConstans
{
    public const string ExchangeRateList = "exchangerates:list";
    public static string ExchangeRate(string currencyCode) => $"exchangerate:{currencyCode.ToUpperInvariant()}";

    public const string CountryList = "countries:list";

    public static string Customer(int customerId) => $"customer:{customerId}";
    public static string CustomerCards(int customerId) => $"customer:{customerId}:cards";
}

// Cache süreleri (TTL): verinin değişme sıklığına göre
public static class CacheDurations
{
    public static readonly TimeSpan ExchangeRate = TimeSpan.FromMinutes(60);
    public static readonly TimeSpan Country = TimeSpan.FromHours(24);
    public static readonly TimeSpan Customer = TimeSpan.FromMinutes(10);
    public static readonly TimeSpan CustomerCards = TimeSpan.FromMinutes(1);
}