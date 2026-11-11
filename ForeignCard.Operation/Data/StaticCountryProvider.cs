using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Data;

public class StaticCountryProvider : ICountryProvider
{
    public Task<List<CountryInfo>> GetCountriesAsync(CancellationToken cancellationToken)
    {
        var list = CountryConstants.Countries
            .Select(c => new CountryInfo
            {
                CountryCode = c.Code,
                CountryName = c.Name,
                CurrencyCode = c.Currency
            })
            .ToList();

        return Task.FromResult(list);
    }
}