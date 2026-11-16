using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.ExchangeRates;
using MediatR;

namespace ForeignCard.Api.BackgroundServices;

public class ExchangeRateJob : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExchangeRateJob> _logger;

    public ExchangeRateJob(IServiceScopeFactory scopeFactory, ILogger<ExchangeRateJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var provider = scope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var rates = await provider.GetRatesAsync(stoppingToken);

                foreach (var rate in rates)
                {
                    await mediator.Send(new SetExchangeRateModel.SetExchangeRateRequest
                    {
                        CurrencyCode = rate.Key,
                        Rate = rate.Value
                    }, stoppingToken);
                }

                // Yeni kurlar yazıldı: eski kurları cache'ten sil,
                // bir sonraki istek veritabanından taze okusun
                await cache.RemoveAsync(CacheKeyConstans.ExchangeRateList);
                foreach (var rate in rates)
                    await cache.RemoveAsync(CacheKeyConstans.ExchangeRate(rate.Key));

                _logger.LogInformation("Kurlar guncellendi: {Count} adet, cache temizlendi", rates.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kur guncelleme hatasi");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}