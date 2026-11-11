using ForeignCard.Common.Abstractions;
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

            _logger.LogInformation("Ulkeler yuklendi: {Count} adet", countries.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine(">>> HATA: " + ex);
            _logger.LogError(ex, "Ulke yukleme hatasi");
        }
    }
}