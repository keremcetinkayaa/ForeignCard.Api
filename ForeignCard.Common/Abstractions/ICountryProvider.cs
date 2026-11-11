namespace ForeignCard.Common.Abstractions;

public interface ICountryProvider
{
    Task<List<CountryInfo>> GetCountriesAsync(CancellationToken cancellationToken);
}

public class CountryInfo
{
    public string CountryCode { get; set; } = "";
    public string CountryName { get; set; } = "";
    public string CurrencyCode { get; set; } = "";
}