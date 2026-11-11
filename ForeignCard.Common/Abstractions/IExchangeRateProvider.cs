namespace ForeignCard.Common.Abstractions;

public interface IExchangeRateProvider
{
    Task<Dictionary<string, decimal>> GetRatesAsync(CancellationToken cancellationToken);
}