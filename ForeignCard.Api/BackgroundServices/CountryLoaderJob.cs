using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Countries;
using MediatR;

namespace ForeignCard.Api.BackgroundServices;

public class CountryLoaderJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CountryLoaderJob> _logger;

    public CountryLoaderJob(IServiceScopeFactory scopeFactory, ILogger<CountryLoaderJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine(">>>CountryLoaderJob baslatildi");
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<ICountryProvider>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

            var countries = await provider.GetCountriesAsync(stoppingToken);

            _logger.LogInformation("Ulkeler yukleniyor: {Count} adet", countries.Count);

            foreach (var c in countries)
            {
                await mediator.Send(new SetCountryModel.SetCountryRequest
                {
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CurrencyCode = c.CurrencyCode
                }, stoppingToken);
            }

            // Ülkeler yazıldı: eski listeyi cache'ten sil
            await cache.RemoveAsync(CacheKeyConstans.CountryList);

            _logger.LogInformation("Ulkeler yuklendi: {Count} adet, cache temizlendi", countries.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine(">>> HATA: " + ex);
            _logger.LogError(ex, "Ulke yukleme hatasi");
        }
    }
}