using ForeignCard.Model.Auth;
using MediatR;

namespace ForeignCard.Api.BackgroundServices;

public class AdminSeederJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<AdminSeederJob> _logger;

    public AdminSeederJob(IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<AdminSeederJob> logger)
    {
        _scopeFactory = scopeFactory;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine(">>> AdminSeederJob basladi");
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new SeedAdminModel.AdminRequest
            {
                NationalId = _config["Admin:NationalId"]!,
                Password = _config["Admin:Password"]!
            }, stoppingToken);

            Console.WriteLine(">>> Admin sonucu: created=" + result.Created);
            _logger.LogInformation("Admin kontrolu: olusturuldu={Created}", result.Created);
        }
        catch (Exception ex)
        {
            Console.WriteLine(">>> Admin olusturma hatasi: " + ex);
        }
    }
}