using System.Globalization;
using System.Xml.Linq;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Common.Helpers;

namespace ForeignCard.Operation.Data;

public class TcmbExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _http;

    public TcmbExchangeRateProvider(HttpClient http)
    {
        _http = http;
    }

    public async Task<Dictionary<string, decimal>> GetRatesAsync(CancellationToken cancellationToken)
    {
        var xml = await _http.GetStringAsync(ExchangeRateConstants.LiveExchangeRate, cancellationToken);
        var doc = XDocument.Parse(xml);

        var result = new Dictionary<string, decimal>();

        foreach (var node in doc.Descendants("Currency"))
        {
            var code = node.Attribute("CurrencyCode")?.Value;
            if (code == null || !CurrencyCode.IsSupported(code))
                continue;

            var selling = node.Element("ForexSelling")?.Value;
            var unitText = node.Element("Unit")?.Value;

            if (!decimal.TryParse(selling, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate) || rate <= 0)
                continue;

            if (!decimal.TryParse(unitText, NumberStyles.Any, CultureInfo.InvariantCulture, out var unit) || unit <= 0)
                unit = 1;

            result[code] = rate / unit;
        }

        return result;
    }
}